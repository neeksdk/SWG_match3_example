using System.Collections.Generic;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Transform _backgroundsTransform;
        [SerializeField] private Transform _tilesTransform;
        
        private BoardData _boardData;
        private BoardTileData[,] _boardTileData;

        private readonly BackgroundTileGenerator _backgroundTileGenerator = new BackgroundTileGenerator();
        private readonly TileGenerator _tileGenerator = new TileGenerator();

        public void SetupLevel(BoardData boardData)
        {
            _boardData = boardData;
            GenerateLevel();
        }

        public void ShuffleBoard() =>
            _tileGenerator.ShuffleBoard(_boardTileData);

        public void ClearBoard()
        {
            foreach (BoardTileData boardTileData in _boardTileData)
            {
                if (!boardTileData.Tile.Recycle())
                {
                    Destroy(boardTileData.Tile.GameObject);
                }

                if (!boardTileData.Background.Recycle())
                {
                    Destroy(boardTileData.Background.GameObject);
                }
            }

            _boardTileData = null;
        }

        private void GenerateLevel()
        {
            _backgroundTileGenerator.GenerateBackground(_boardData, _backgroundsTransform, out BoardTileData[,] boardTileData);
            _boardTileData = boardTileData;
            
            _tileGenerator.GenerateTiles(_boardData, _boardTileData, _tilesTransform);
        }
        
        private bool FindMatchedTiles(BoardTileData boardTileData, BoardSearchPattern searchPattern, out List<BoardTileData> matchedTiles)
        {
            matchedTiles = new List<BoardTileData>();
            TileType tileType = boardTileData.Tile.TileType;
            switch (searchPattern)
            {
                case BoardSearchPattern.Horizontal:
                    bool checkRight = CheckTileForMatchRecursively(tileType, boardTileData.Row + 1, boardTileData.Col, ref matchedTiles, 1, 0);
                    bool checkLeft = CheckTileForMatchRecursively(tileType, boardTileData.Row - 1, boardTileData.Col, ref matchedTiles, -1, 0);
                    break;
                case BoardSearchPattern.Vertical:
                    bool checkUp = CheckTileForMatchRecursively(tileType, boardTileData.Row, boardTileData.Col + 1, ref matchedTiles, 0, 1);
                    bool checkDown = CheckTileForMatchRecursively(tileType, boardTileData.Row, boardTileData.Col - 1, ref matchedTiles, 0, -1);
                    break;
                default:
                    break;
            }

            return matchedTiles.Count >= 3;
        }

        private bool FindMatchesCountAfterTileSwap(BoardTileData target, BoardTileData destination, out BoardSearchPattern matchPattern)
        {
            matchPattern = BoardSearchPattern.Both;
            
            BoardTileData targetTile = target;
            BoardTileData destinationTile = destination;

            _boardTileData[target.Row, target.Col] = destination;
            _boardTileData[destination.Row, destination.Col] = target;
            
            //todo: check if match occurs
            
            _boardTileData[target.Row, target.Col] = target;
            _boardTileData[destination.Row, destination.Col] = destination;

            return false;
        }


        private bool CheckTileForMatchRecursively(TileType matchedType, int row, int col, ref List<BoardTileData> matchedTiles, int incrementRow, int incrementCol)
        {
            while (true)
            {
                if (!_boardData.IsInsideGridBounds(row, col))
                {
                    return false;
                }

                BoardTileData nextTile = _boardTileData[row, col];
                if (nextTile.Tile == null || nextTile.Tile.TileType != matchedType)
                {
                    return false;
                }

                matchedTiles.Add(nextTile);
                row += incrementRow;
                col += incrementCol;
            }
        }
    }
}
