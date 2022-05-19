using neeksdk.Scripts.Extensions;
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
            BoardCoords fromCoords = fromTileData.Coords;
            BoardTileData toTileData = this.GetBoardTileData(toTile);
            BoardCoords toCoords = toTileData.Coords;
            
            return Promise.All(fromTile.Move(toTile.Coords), toTile.Move(fromTile.Coords)).Then(() =>
            {
                fromTileData.Tile = toTile;
                fromTileData.Coords = toCoords;
                toTileData.Tile = fromTile;
                toTileData.Coords = fromCoords;
            });
        }

        public void ShuffleBoard() =>
            _tileGenerator.ShuffleBoard(BoardTileData);

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
                return _tileGenerator.GenerateTiles(BoardData, BoardTileData, _tilesTransform);
            });
    }
}
