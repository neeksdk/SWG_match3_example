using System;
using System.Collections.Generic;
using System.Linq;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using RSG;
using UnityEngine;
using Random = UnityEngine.Random;

namespace neeksdk.Scripts.Game.Board
{
    public class TileGenerator
    {
        public IPromise GenerateAllLevelTiles(BoardData boardData, BoardTileData[,] boardTileData, Transform transform)
        {
            List<Func<IPromise>> promises = new List<Func<IPromise>>();
            for (int j = 0; j < boardData.Cols; j++)
            {
                for (int i = 0; i < boardData.Rows; i++)
                {
                    BoardTileData generateTileData = boardTileData[i, j];

                    if (generateTileData == null || generateTileData.BackgroundType == BackgroundType.Empty)
                    {
                        continue;
                    }

                    List<TileType> allowedTileTypes = boardData.TileTypes.ToList();
                    BoardTileData left2 = i >= 2 ? boardTileData[i - 2, j] : null;
                    BoardTileData left1 = i >= 1 ? boardTileData[i - 1, j] : null;
                    BoardTileData down2 = j >= 2 ? boardTileData[i, j - 2] : null;
                    BoardTileData down1 = j >= 1 ? boardTileData[i, j - 1] : null;

                    if (left2 != null && left1 != null && left2.BackgroundType != BackgroundType.Empty && left1.BackgroundType != BackgroundType.Empty)
                    {
                        if (left2.TileType() == left1.TileType())
                        {
                            allowedTileTypes.Remove(left2.TileType());
                        }
                    }

                    if (down2 != null && down1 != null && down2.BackgroundType != BackgroundType.Empty && down1.BackgroundType != BackgroundType.Empty)
                    {
                        if (down2.TileType() == down1.TileType())
                        {
                            allowedTileTypes.Remove(down2.TileType());
                        }
                    }

                    ITile tile = GenerateTileForBoardData(generateTileData, allowedTileTypes, transform);
                    if (tile != null)
                    {
                        Func<IPromise> promise = new Func<IPromise>(() => tile.ShowUp());
                        promises.Add(promise);
                    }
                }
            }

            return Promise.Sequence(promises);
        }

        public ITile GenerateNewTile(List<TileType> allowedTileTypes, Transform transform, BoardCoords initialCoords)
        {
            int randomIndex = Random.Range(0, allowedTileTypes.Count);
            TileType randomTileType = allowedTileTypes[randomIndex];
            
            if (!randomTileType.Spawn(transform, out ITile newTile, initialCoords.BoardToVectorCoords()))
            {
                return null;
            }
            
            newTile.Coords = initialCoords;
            newTile.GameObject.transform.localScale = Vector3.one;
            newTile.GameObject.SetActive(true);
            
            return newTile;
        }
        
        public void ShuffleBoard(BoardTileData[,] boardTileData)
        {
            //todo: implement board shuffle
        }

        private ITile GenerateTileForBoardData(BoardTileData boardTileData, List<TileType> allowedTileTypes, Transform transform)
        {
            int randomIndex = Random.Range(0, allowedTileTypes.Count);
            TileType randomTileType = allowedTileTypes[randomIndex];

            if (!randomTileType.Spawn(transform, out ITile tile, boardTileData.BoardToVectorCoords()))
            {
                return null;
            }
            
            boardTileData.Tile = tile;
            tile.Coords = boardTileData.Coords;
            tile.GameObject.transform.localScale = Vector3.zero;
            tile.GameObject.SetActive(true);

            return tile;
        }
    }
}