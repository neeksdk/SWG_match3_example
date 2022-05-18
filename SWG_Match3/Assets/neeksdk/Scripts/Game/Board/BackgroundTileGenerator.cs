using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class BackgroundTileGenerator
    {
        public void GenerateBackground(BoardData boardData, Transform transform, out BoardTileData[,] boardTileData)
        {
            boardTileData = new BoardTileData[boardData.Rows, boardData.Cols];
            
            PopulateBoardData(boardData, boardTileData);
            AddEmptyTiles(boardData, boardTileData);
            PopulateBackgroundTiles(boardTileData, transform);
        }

        private void PopulateBoardData(BoardData boardData, BoardTileData[,] boardTileData)
        {
            for (int i = 0; i < boardData.Rows; i++)
            {
                for (int j = 0; j < boardData.Cols; j++)
                {
                    boardTileData[i, j] = new BoardTileData()
                    {
                        Row = i,
                        Col = j,
                        BackgroundType = BackgroundType.Standard
                    };
                }
            }
        }

        private void AddEmptyTiles(BoardData boardData, BoardTileData[,] boardTileData)
        {
            int index = boardData.EmptyTiles;
            while (index != 0)
            {
                int randomRow = Random.Range(0, boardData.Rows);
                int randomCol = Random.Range(0, boardData.Cols);
                if (boardTileData[randomRow, randomCol].BackgroundType == BackgroundType.Empty)
                {
                    continue;
                }

                boardTileData[randomRow, randomCol].BackgroundType = BackgroundType.Empty;
                index -= 1;
            }
        }

        private void PopulateBackgroundTiles(BoardTileData[,] boardTileData, Transform transform)
        {
            foreach (BoardTileData tileData in boardTileData)
            {
                if (!tileData.BackgroundType.Spawn(transform, out IBackground background, tileData.BoardToVectorCoords()))
                {
                    continue;
                }

                tileData.Background = background;
                background.GameObject.SetActive(true);
            }
        }
    }
}