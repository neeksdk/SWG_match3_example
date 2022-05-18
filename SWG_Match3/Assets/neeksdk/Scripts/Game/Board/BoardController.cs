using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class BoardController : MonoBehaviour
    {
        [SerializeField] private Transform _backgroundsTransform;
        [SerializeField] private Transform _tilesTransform;
        
        private BoardData _boardData;
        private BoardTileData[,] _boardTileData;

        public void GenerateLevel(int rows, int cols, int emptyTiles, params TileType[] tileTypes)
        {
            _boardData = new BoardData()
            {
                Rows = rows,
                Cols = cols,
                EmptyTiles = emptyTiles,
                TileTypes = tileTypes
            };
            
            GenerateBackground();
        }

        private void GenerateBackground()
        {
            _boardTileData = new BoardTileData[_boardData.Rows, _boardData.Cols];
            
            PopulateBoardData();
            AddEmptyTiles();
            PopulateBackgroundTiles();
        }

        private void PopulateBoardData()
        {
            for (int i = 0; i < _boardData.Rows; i++)
            {
                for (int j = 0; j < _boardData.Cols; j++)
                {
                    _boardTileData[i, j] = new BoardTileData()
                    {
                        Row = i,
                        Col = j,
                        BackgroundType = BackgroundType.Standard
                    };
                }
            }
        }

        private void AddEmptyTiles()
        {
            int index = _boardData.EmptyTiles;
            while (index != 0)
            {
                int randomRow = Random.Range(0, _boardData.Rows);
                int randomCol = Random.Range(0, _boardData.Cols);
                if (_boardTileData[randomRow, randomCol].BackgroundType == BackgroundType.Empty)
                {
                    continue;
                }

                _boardTileData[randomRow, randomCol].BackgroundType = BackgroundType.Empty;
                index -= 1;
            }
        }

        private void PopulateBackgroundTiles()
        {
            foreach (BoardTileData boardTileData in _boardTileData)
            {
                if (!boardTileData.BackgroundType.Spawn(_backgroundsTransform, out IBackground background, BoardToVectorCoords(boardTileData)))
                {
                    continue;
                }

                boardTileData.Background = background;
                background.GameObject.SetActive(true);
            }
        }

        private Vector3 BoardToVectorCoords(BoardTileData boardTileData) =>
            new Vector3(boardTileData.Row, boardTileData.Col, 0);

        private class BoardData
        {
            public int Rows;
            public int Cols;
            public int EmptyTiles;
            public TileType[] TileTypes;
        }

        private class BoardTileData
        {
            public int Row;
            public int Col;
            public BackgroundType BackgroundType;
            public IBackground Background;
        }
    }
}
