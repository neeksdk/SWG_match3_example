using System;
using System.Collections.Generic;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;

namespace neeksdk.Scripts.Game.Board.BoardControl
{
    public class BoardMatcher
    {
        private readonly BoardController _boardController;

        public BoardMatcher(BoardController boardController) =>
            _boardController = boardController;

        public bool TryToFindMatchesOnAllBoard(out List<BoardTileData> matchedTiles)
        {
            matchedTiles = new List<BoardTileData>();
            FindMatchesInRows(matchedTiles);
            FindMatchesInCols(matchedTiles);
            
            return matchedTiles.Count > 0;
        }

        public bool FindMatchedTiles(BoardTileData boardTileData, BoardSearchPattern searchPattern, out List<BoardTileData> matchedTiles)
        {
            matchedTiles = new List<BoardTileData>();
            matchedTiles.Add(boardTileData);
            TileType tileType = boardTileData.Tile.TileType;
            BoardCoords coords = boardTileData.Coords;
            switch (searchPattern)
            {
                case BoardSearchPattern.Vertical:
                    CheckTileForMatchRecursively(tileType, coords.Row + 1, coords.Col, matchedTiles, 1, 0);
                    CheckTileForMatchRecursively(tileType, coords.Row - 1, coords.Col, matchedTiles, -1, 0);
                    break;
                case BoardSearchPattern.Horizontal:
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col + 1, matchedTiles, 0, 1);
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col - 1, matchedTiles, 0, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchPattern), searchPattern, null);
            }

            return matchedTiles.Count >= 3;
        }

        public bool IsAvailableMovesOnBoard()
        {
            for (int i = 1; i < _boardController.BoardData.Rows; i++)
            {
                for (int j = 0; j < _boardController.BoardData.Cols; j++)
                {
                    BoardTileData fromTile = _boardController.BoardTileData[i - 1, j];
                    BoardTileData toTile = _boardController.BoardTileData[i, j];
                    if (GetAvailableMoves(fromTile, toTile) > 0)
                    {
                        return true;
                    }
                }
            }
            
            for (int j = 1; j < _boardController.BoardData.Cols; j++)
            {
                for (int i = 0; i < _boardController.BoardData.Rows; i++)
                {
                    BoardTileData fromTile = _boardController.BoardTileData[i, j - 1];
                    BoardTileData toTile = _boardController.BoardTileData[i, j];
                    if (GetAvailableMoves(fromTile, toTile) > 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        
        private int GetAvailableMoves(BoardTileData fromTileData, BoardTileData toTileData)
        {
            if (fromTileData.BackgroundType == BackgroundType.Empty || toTileData.BackgroundType == BackgroundType.Empty)
            {
                return 0;
            }

            if (fromTileData.Tile == null || toTileData.Tile == null)
            {
                return 0;
            }

            int availableMoves = 0;

            availableMoves = CheckAvailableMoves(fromTileData, toTileData, availableMoves, TileMoveDirections.Up);
            availableMoves = CheckAvailableMoves(fromTileData, toTileData, availableMoves, TileMoveDirections.Down);
            availableMoves = CheckAvailableMoves(fromTileData, toTileData, availableMoves, TileMoveDirections.Right);
            availableMoves = CheckAvailableMoves(fromTileData, toTileData, availableMoves, TileMoveDirections.Left);

            return availableMoves;
        }

        private int CheckAvailableMoves(BoardTileData fromTileData, BoardTileData toTileData, int availableMoves, TileMoveDirections tileMoveDirections)
        {
            availableMoves = GetAvailableMoves(fromTileData, toTileData, availableMoves, GetCoordsForMoveDirection(tileMoveDirections));
            
            return availableMoves;
        }

        private int GetAvailableMoves(BoardTileData fromTileData, BoardTileData toTileData, int availableMoves, BoardCoords upTileCoords)
        {
            List<BoardTileData> matchedTiles = new List<BoardTileData>();
            CheckTileForMatchRecursively(toTileData.TileType(), fromTileData.Coords.Row + upTileCoords.Row,
                fromTileData.Coords.Col + upTileCoords.Col, matchedTiles, upTileCoords.Row, upTileCoords.Col);
            
            if (matchedTiles.Count >= 2)
            {
                availableMoves += 1;
            }

            matchedTiles.Clear();
            return availableMoves;
        }

        private bool CheckTileForMatchRecursively(TileType matchedType, int row, int col, List<BoardTileData> matchedTiles, int incrementRow, int incrementCol)
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
                
                matchedTiles?.Add(nextTile);
                
                return CheckTileForMatchRecursively(matchedType, row + incrementRow, col + incrementCol, matchedTiles, incrementRow, incrementCol);
            }
        }
        
        private void FindMatchesInRows(List<BoardTileData> matchedTiles)
        {
            TileType findingTileType = TileType.Fire;
            List<BoardTileData> findingMatches = new List<BoardTileData>();
            for (int i = 0; i < _boardController.BoardData.Rows; i++)
            {
                for (int j = 0; j < _boardController.BoardData.Cols; j++)
                {
                    findingTileType = FindingMatchedTiles(matchedTiles, findingMatches, findingTileType, i, j);
                }
                
                if (findingMatches.Count >= 3)
                {
                    matchedTiles.AddRange(findingMatches);
                }
                
                findingMatches.Clear();
            }
        }
        
        private void FindMatchesInCols(List<BoardTileData> matchedTiles)
        {
            TileType findingTileType = TileType.Fire;
            List<BoardTileData> findingMatches = new List<BoardTileData>();
            for (int j = 0; j < _boardController.BoardData.Cols; j++)
            {
                
                for (int i = 0; i < _boardController.BoardData.Rows; i++)
                {
                    findingTileType = FindingMatchedTiles(matchedTiles, findingMatches, findingTileType, i, j);
                }
                
                if (findingMatches.Count >= 3)
                {
                    matchedTiles.AddRange(findingMatches);
                }
                findingMatches.Clear();
            }
        }

        private TileType FindingMatchedTiles(List<BoardTileData> matchedTiles, List<BoardTileData> findingMatches, TileType findingTileType, int i, int j)
        {
            BoardTileData tileData = _boardController.BoardTileData[i, j];
            if (tileData == null || tileData.BackgroundType == BackgroundType.Empty)
            {
                if (findingMatches.Count >= 3)
                {
                    matchedTiles.AddRange(findingMatches);
                }
                
                findingMatches.Clear();
                return findingTileType;
            }

            if (findingMatches.Count == 0)
            {
                findingTileType = tileData.TileType();
                findingMatches.Add(tileData);
                return findingTileType;
            }

            if (findingTileType == tileData.TileType())
            {
                findingMatches.Add(tileData);
                return findingTileType;
            }

            if (findingMatches.Count >= 3)
            {
                matchedTiles.AddRange(findingMatches);
            }

            findingMatches.Clear();
            findingTileType = tileData.TileType();
            findingMatches.Add(tileData);
            
            return findingTileType;
        }
        
        private BoardCoords GetCoordsForMoveDirection(TileMoveDirections tileMoveDirection)
        {
            int incrementRow = 0;
            int incrementCol = 0;

            switch (tileMoveDirection)
            {
                case TileMoveDirections.Up:
                    incrementRow = 1;
                    break;
                case TileMoveDirections.Right:
                    incrementCol = 1;
                    break;
                case TileMoveDirections.Down:
                    incrementRow = -1;
                    break;
                case TileMoveDirections.Left:
                    incrementCol = -1;
                    break;
                case TileMoveDirections.None:
                default:
                    break;
            }

            return new BoardCoords() {Row = incrementRow, Col = incrementCol};
        }
    }
}