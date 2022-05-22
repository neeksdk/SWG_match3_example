using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board
{
    public class BackgroundTileGenerator
    {
        public IPromise<BoardTileData[,]> GenerateBackground(BoardData boardData, Transform backgroundTransform, Transform tileGenerationContainerTransform)
        {
            BoardTileData[,] boardTileData = new BoardTileData[boardData.Rows, boardData.Cols];
            
            PopulateBoardData(boardData, boardTileData);
            AddEmptyTiles(boardData, boardTileData);
            AddTileGenerationContainers(boardData, tileGenerationContainerTransform);
            PopulateBackgroundTiles(boardTileData, backgroundTransform);

            return Promise<BoardTileData[,]>.Resolved(boardTileData);
        }

        private void PopulateBoardData(BoardData boardData, BoardTileData[,] boardTileData)
        {
            for (int i = 0; i < boardData.Rows; i++)
            {
                for (int j = 0; j < boardData.Cols; j++)
                {
                    boardTileData[i, j] = new BoardTileData()
                    {
                        Coords = new BoardCoords()
                        {
                            Row = i,
                            Col = j
                        },
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
        
        private void AddTileGenerationContainers(BoardData boardData, Transform tileGenerationContainerTransform)
        {
            for (int i = 0; i < boardData.Rows; i++)
            {
                BoardCoords containerPosition = new BoardCoords() {Row = i, Col = -1};
                BackgroundType containerType = BackgroundType.Empty;
                if (!containerType.Spawn(tileGenerationContainerTransform, out IBackground background, containerPosition.BoardToVectorCoords()))
                {
                    continue;
                }
                
                background.GameObject.SetActive(true);
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