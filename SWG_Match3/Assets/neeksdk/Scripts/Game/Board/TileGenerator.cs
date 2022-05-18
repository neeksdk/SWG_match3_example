using System.Collections.Generic;
using System.Linq;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class TileGenerator
    {
        public void GenerateTiles(BoardData boardData, BoardTileData[,] boardTileData, TileType[] tileTypes, Transform transform)
        {
            BoardTileData left2;
            BoardTileData left1;
            BoardTileData down2;
            BoardTileData down1;

            for (int i = 0; i < boardData.Rows; i++)
            {
                for (int j = 0; j < boardData.Cols; j++)
                {
                    BoardTileData generateTileData = boardTileData[i, j];

                    if (generateTileData == null || generateTileData.BackgroundType == BackgroundType.Empty)
                    {
                        continue;
                    }

                    List<TileType> allowedTileTypes = tileTypes.ToList();
                    left2 = i >= 2 ? boardTileData[i - 2, j] : null;
                    left1 = i >= 1 ? boardTileData[i - 1, j] : null;
                    down2 = j >= 2 ? boardTileData[i, j - 2] : null;
                    down1 = j >= 1 ? boardTileData[i, j - 1] : null;

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
                    
                    GenerateTile(generateTileData, allowedTileTypes, transform);
                }
            }
        }

        public void ShuffleBoard(BoardTileData[,] boardTileData)
        {
            //todo: implement board shuffle
        }

        private void GenerateTile(BoardTileData boardTileData, List<TileType> allowedTileTypes, Transform transform)
        {
            int randomIndex = Random.Range(0, allowedTileTypes.Count);
            TileType randomTileType = allowedTileTypes[randomIndex];

            if (randomTileType.Spawn(transform, out ITile tile, boardTileData.BoardToVectorCoords()))
            {
                boardTileData.Tile = tile;
                tile.GameObject.SetActive(true);
            }
        }
    }
}