using System.Collections.Generic;
using System.Linq;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Transform _backgroundsTransform;
        [SerializeField] private Transform _tilesTransform;

        private BackgroundTileGenerator _backgroundTileGenerator;
        private TileGenerator _tileGenerator;
        
        public BoardMatcher BoardMatcher { get; private set; }
        public BoardTileData[,] BoardTileData { get; private set; }
        public BoardData BoardData { get; private set; }

        public IPromise SetupLevel(BoardData boardData)
        {
            BoardData = boardData;
            return GenerateLevel();
        }

        public IPromise SwapTiles(ITile fromTile, ITile toTile)
        {
            fromTile.Deselect();
            toTile.Deselect();
            
            BoardTileData fromTileData = this.GetBoardTileData(fromTile);
            BoardTileData toTileData = this.GetBoardTileData(toTile);
            
            return Promise.All(fromTile.Move(toTileData.Coords), toTile.Move(fromTileData.Coords)).Then(() =>
            {
                fromTileData.Tile = toTile;
                toTileData.Tile = fromTile;
            });
        }

        public void ShuffleBoard() =>
            _tileGenerator.ShuffleBoard(BoardTileData);

        public bool TryGetAllEmptyTiles(out List<BoardTileData> boardTileData)
        {
            boardTileData = new List<BoardTileData>();
            for (int row = BoardData.Rows - 1; row >= 0 ; row--)
            {
                List<BoardTileData> emptyTiles = new List<BoardTileData>();
                List<ITile> filledTiles = new List<ITile>();
                PopulateEmptyTilesInfo(row, emptyTiles, filledTiles);

                if (emptyTiles.Count == 0)
                {
                    continue;
                }
                
                GenerateNewTiles(row, filledTiles, emptyTiles.Count - filledTiles.Count);
                
                for (int i = 0; i < emptyTiles.Count; i++)
                {
                    BoardTileData emptyBoardTile = emptyTiles[i];
                    emptyBoardTile.Tile = filledTiles[i];
                    boardTileData.Add(emptyBoardTile);
                }
            }

            return boardTileData.Count > 0;
        }

        public void ClearBoard()
        {
            foreach (BoardTileData boardTileData in BoardTileData)
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

            BoardTileData = null;
        }

        private void Awake()
        {
            _backgroundTileGenerator = new BackgroundTileGenerator();
            _tileGenerator = new TileGenerator();
            BoardMatcher = new BoardMatcher(this);
        }

        private IPromise GenerateLevel() =>
            _backgroundTileGenerator.GenerateBackground(BoardData, _backgroundsTransform).Then(boardTileData =>
            {
                BoardTileData = boardTileData;
                return _tileGenerator.GenerateAllLevelTiles(BoardData, BoardTileData, _tilesTransform);
            });
        
        private void GenerateNewTiles(int row, List<ITile> filledTiles, int emptyTilesCount)
        {
            for (int i = 0; i < emptyTilesCount; i++)
            {
                ITile newTile = _tileGenerator.GenerateNewTile(BoardData.TileTypes.ToList(), _tilesTransform, new BoardCoords() {Row = row, Col = -(i + 1)});

                if (newTile != null)
                {
                    filledTiles.Add(newTile);
                }
            }
        }

        private void PopulateEmptyTilesInfo(int row, List<BoardTileData> emptyTiles, List<ITile> filledTiles)
        {
            for (int col = BoardData.Cols - 1; col >= 0; col--)
            {
                BoardTileData currentTileData = BoardTileData[row, col];

                if (currentTileData.BackgroundType == BackgroundType.Empty)
                {
                    continue;
                }
                
                if (currentTileData.Tile == null)
                {
                    emptyTiles.Add(currentTileData);
                    continue;
                }

                if (emptyTiles.Count <= 0)
                {
                    continue;
                }
                
                filledTiles.Add(currentTileData.Tile);
                emptyTiles.Add(currentTileData);
            }
        }
    }
}
