using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Constants;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board;
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
            if (Camera.main == null)
            {
                return Promise.Resolved();
            }
            
            Vector3 scorePosition = Camera.main.ScreenToWorldPoint(_gameUiView.ScorePosition);
            List<IPromise> promises = new List<IPromise>();

            while (_completeAnimationQueue.Count != 0)
            {
                List<BoardTileData> boardTileDataList = _completeAnimationQueue.Dequeue();
                int scorePerTile = 5;
                int scoreForFirstTile = 4;
                int scoreForSecondAndThirdTile = 3;
                
                for (int i = 0; i < boardTileDataList.Count; i++)
                {
                    BoardTileData boardTileData = boardTileDataList[i];
                    AnimationTileData animationTileData = new AnimationTileData()
                    {
                        Transform = boardTileData.Tile.TileMonoContainer.transform,
                        Score = i == 0 ? scoreForFirstTile : i < 3 ? scoreForSecondAndThirdTile : scorePerTile
                    };

                    promises.Add(GetAnimationPromise(animationTileData, scorePosition, TileConstants.TILE_SELECTION_ANIMATION_DELAY * i)
                        .Then(() =>
                        {
                            boardTileData.Tile.Recycle();
                            boardTileData.Tile = null;
                        }));
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
                
                animationPromises.Add(boardTileData.Tile.Move(newPosition));
            }

            return Promise.All(animationPromises).Then(Promise.Resolved);
        }

        private IPromise GetAnimationPromise(AnimationTileData animationTileData, Vector3 scorePosition, float delay)
        {
            Promise promise = new Promise();
            animationTileData.Transform.DOKill();
            DOVirtual.DelayedCall(delay, () =>
            {
                Sequence animationSequence = DOTween.Sequence();
                animationSequence.Append(animationTileData.Transform.DOMove(scorePosition, TileConstants.TILE_MOVE_ANIMATION_DURATION)
                    .SetEase(Ease.OutSine)
                    .OnComplete(() => _gameUiView.AnimateScorePoints(animationTileData.Score)));
                animationSequence.Append(animationTileData.Transform.DOScale(Vector3.zero, TileConstants.TILE_MOVE_ANIMATION_DURATION)
                    .SetEase(Ease.OutSine));
                animationSequence.OnComplete(() => promise.Resolve());
            });
            
            return promise;
        }
    }
}