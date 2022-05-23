using System.Collections.Generic;
using System.Linq;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Game.GameUIView;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.Services
{
    public class TileAnimationService
    {
        private readonly GameUiView _gameUiView;
        private readonly Queue<List<BoardTileData>> _completeAnimationQueue = new Queue<List<BoardTileData>>();

        public TileAnimationService(GameUiView gameUiView) =>
            _gameUiView = gameUiView;

        public void AddCollectAnimationsToQueue(List<BoardTileData> animatedTiles) =>
            _completeAnimationQueue.Enqueue(animatedTiles);

        public bool HasAnimations() =>
            _completeAnimationQueue.Count > 0;

        public void ClearCollectAnimationQueue() =>
            _completeAnimationQueue.Clear();

        public IPromise PlayCollectTileAnimations()
        {
            if (Camera.main == null || _completeAnimationQueue.Count == 0)
            {
                return Promise.Resolved();
            }

            Vector3 scorePosition = Camera.main.ScreenToWorldPoint(_gameUiView.ScorePosition);
            List<IPromise> promises = new List<IPromise>();
            int scoreIndex = 0;

            while (_completeAnimationQueue.Count > 0)
            {
                List<BoardTileData> boardTileDataList = _completeAnimationQueue.Dequeue().Distinct().ToList();
                int scorePerTile = 5;
                int scoreForFirstTile = 4;
                int scoreForSecondAndThirdTile = 3;

                for (int i = 0; i < boardTileDataList.Count; i++)
                {
                    BoardTileData boardTileData = boardTileDataList[i];
                    ITile tile = boardTileData.Tile;
                    if (tile == null)
                    {
                        continue;
                    }

                    boardTileData.Tile = null;
                    int score = scoreIndex == 0 ? scoreForFirstTile : scoreIndex < 3 ? scoreForSecondAndThirdTile : scorePerTile;
                    scoreIndex += 1;
                    promises.Add(tile.Collect(scorePosition, () =>
                    {
                        _gameUiView.AnimateScorePoints(score);
                    }).Then(() => tile.Recycle()));
                }
            }
            
            return Promise.All(promises);
        }

        public IPromise MoveTilesOnBoard(List<BoardTileData> animatedTiles)
        {
            List<IPromise> animationPromises = new List<IPromise>();
            foreach (BoardTileData boardTileData in animatedTiles)
            {
                BoardCoords newPosition = boardTileData.Tile.Coords;
                BoardCoords finalPosition = boardTileData.Coords;
                
                if (finalPosition.Row != newPosition.Row)
                {
                    newPosition.Row += newPosition.Row < finalPosition.Row ? 1 : -1;
                }
                
                if (finalPosition.Col != newPosition.Col)
                {
                    newPosition.Col += newPosition.Col < finalPosition.Col ? 1 : -1;
                }

                if (newPosition.Row < 0 || newPosition.Col < 0)
                {
                    boardTileData.Tile.Coords = newPosition;
                }
                else
                {
                    animationPromises.Add(boardTileData.Tile.Move(newPosition));
                }
            }

            return Promise.All(animationPromises);
        }
    }
}