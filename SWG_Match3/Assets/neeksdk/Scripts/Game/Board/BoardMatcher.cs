using System;
using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game.Board
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
                case BoardSearchPattern.Horizontal:
                    CheckTileForMatchRecursively(tileType, coords.Row + 1, coords.Col, ref matchedTiles, 1, 0);
                    CheckTileForMatchRecursively(tileType, coords.Row - 1, coords.Col, ref matchedTiles, -1, 0);
                    break;
                case BoardSearchPattern.Vertical:
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col + 1, ref matchedTiles, 0, 1);
                    CheckTileForMatchRecursively(tileType, coords.Row, coords.Col - 1, ref matchedTiles, 0, -1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(searchPattern), searchPattern, null);
            }

            if (matchedTiles.Count >= 3)
            {
                foreach (BoardTileData matchedTile in matchedTiles)
                {
                    matchedTile.Tile.TileMonoContainer.transform.DOScale(0.5f, 0.5f);
                }
            }

            return matchedTiles.Count >= 3;
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
                
                return CheckTileForMatchRecursively(matchedType, row, col, ref matchedTiles, incrementRow, incrementCol);
            }
        }
        
        private void FindMatchesInRows(List<BoardTileData> matchedTiles)
        {
            TileType findingTileType = TileType.Fire;
            List<BoardTileData> findingMatches = new List<BoardTileData>();
            for (int i = 0; i < _boardController.BoardData.Rows; i++)
            {
                findingMatches.Clear();
                for (int j = 0; j < _boardController.BoardData.Cols; j++)
                {
                    findingTileType = FindingMatchedTiles(matchedTiles, findingMatches, findingTileType, i, j);
                }
            }
        }
        
        private void FindMatchesInCols(List<BoardTileData> matchedTiles)
        {
            TileType findingTileType = TileType.Fire;
            List<BoardTileData> findingMatches = new List<BoardTileData>();
            for (int j = 0; j < _boardController.BoardData.Cols; j++)
            {
                findingMatches.Clear();
                for (int i = 0; i < _boardController.BoardData.Rows; i++)
                {
                    findingTileType = FindingMatchedTiles(matchedTiles, findingMatches, findingTileType, i, j);
                }
            }
        }

        private TileType FindingMatchedTiles(List<BoardTileData> matchedTiles, List<BoardTileData> findingMatches, TileType findingTileType, int i, int j)
        {
            BoardTileData tileData = _boardController.BoardTileData[i, j];
            if (tileData.BackgroundType == BackgroundType.Empty)
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
    }
}