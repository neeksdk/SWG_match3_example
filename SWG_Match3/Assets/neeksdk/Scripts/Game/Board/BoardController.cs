using neeksdk.Scripts.Extensions;
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
        private BoardMatcher _boardMatcher;

        public BoardTileData[,] BoardTileData { get; private set; }
        public BoardData BoardData { get; private set; }

        public IPromise SetupLevel(BoardData boardData)
        {
            BoardData = boardData;
            return GenerateLevel();
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
            _boardMatcher = new BoardMatcher(this);
        }

        private IPromise GenerateLevel() =>
            _backgroundTileGenerator.GenerateBackground(BoardData, _backgroundsTransform).Then(boardTileData =>
            {
                BoardTileData = boardTileData;
                return _tileGenerator.GenerateTiles(BoardData, BoardTileData, _tilesTransform);
            });
    }
}
