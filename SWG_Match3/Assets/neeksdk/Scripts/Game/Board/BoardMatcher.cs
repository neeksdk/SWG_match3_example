using System;
using System.Collections.Generic;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game.Board
{
    public class BoardMatcher
    {
        private readonly BoardController _boardController;

        public BoardMatcher(BoardController boardController) =>
            _boardController = boardController;

        public bool FindMatchedTiles(BoardTileData boardTileData, BoardSearchPattern searchPattern, out List<BoardTileData> matchedTiles)
        {
            matchedTiles = new List<BoardTileData>();
            TileType tileType = boardTileData.Tile.TileType;
            BoardCoords coords = boardTileData.Coords;
            switch (searchPattern)
            {
                case BoardSearchPattern.Horizontal:
                    CheckTileForMatchRecursively(tileType, coords.Row + 1, coords.Col, ref matchedTiles, 1, 0);
                    CheckTileForMatchRecursively(tileType, coords.Row - 1, coords.Col, ref matchedTiles, -1, 0);
                    break;
                case BoardSearchPattern.Vertical:
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col + 1, ref matchedTiles, 0, 1);
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col - 1, ref matchedTiles, 0, -1);
                    break;
                case BoardSearchPattern.Both:
                    CheckTileForMatchRecursively(tileType, coords.Row + 1, coords.Col, ref matchedTiles, 1, 0);
                    CheckTileForMatchRecursively(tileType, coords.Row - 1, coords.Col, ref matchedTiles, -1, 0);
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col + 1, ref matchedTiles, 0, 1);
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col - 1, ref matchedTiles, 0, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchPattern), searchPattern, null);
            }

            return matchedTiles.Count >= 3;
        }

        public bool FindMatchesCountAfterTileSwap(BoardTileData target, BoardTileData destination, out BoardSearchPattern matchPattern)
        {
            matchPattern = BoardSearchPattern.Both;
            
            BoardTileData targetTile = target;
            BoardTileData destinationTile = destination;

            _boardController.BoardTileData[target.Coords.Row, target.Coords.Col] = destination;
            _boardController.BoardTileData[destination.Coords.Row, destination.Coords.Col] = target;
            
            //todo: check if match occurs
            
            _boardController.BoardTileData[target.Coords.Row, target.Coords.Col] = target;
            _boardController.BoardTileData[destination.Coords.Row, destination.Coords.Col] = destination;

            return false;
        }


        private bool CheckTileForMatchRecursively(TileType matchedType, int row, int col, ref List<BoardTileData> matchedTiles, int incrementRow, int incrementCol)
        {
            while (true)
            {
                if (!_boardController.BoardData.IsInsideGridBounds(row, col))
                {
                    return false;
                }

                BoardTileData nextTile = _boardController.BoardTileData[row, col];
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