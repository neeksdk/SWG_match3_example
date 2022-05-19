using System.Collections.Generic;
using DG.Tweening;
using neeksdk.Scripts.Constants;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.GameUIView;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.Services
{
    public class TileAnimationService
    {
        private readonly GameUiView _gameUiView;
        private readonly Queue<List<BoardTileData>> _animationQueue = new Queue<List<BoardTileData>>();

        public TileAnimationService(GameUiView gameUiView)
        {
            _gameUiView = gameUiView;
        }

        public void AddAnimationsToQueue(List<BoardTileData> animatedTiles) =>
            _animationQueue.Enqueue(animatedTiles);

        public bool HasAnimations() =>
            _animationQueue.Count > 0;

        public IPromise PlayCollectTileAnimations()
        {
            if (Camera.main == null)
            {
                return Promise.Resolved();
            }
            
            Vector3 scorePosition = Camera.main.ScreenToWorldPoint(_gameUiView.ScorePosition);
            List<IPromise> promises = new List<IPromise>();

            while (_animationQueue.Count != 0)
            {
                List<BoardTileData> boardTileData = _animationQueue.Dequeue();
                int scorePerTile = 5;
                int scoreForFirstTile = 4;
                int scoreForSecondAndThirdTile = 3;
                
                for (int i = 0; i < boardTileData.Count; i++)
                {
                    AnimationTileData animationTileData = new AnimationTileData()
                    {
                        Transform = boardTileData[i].Tile.TileMonoContainer.transform,
                        Score = i == 0 ? scoreForFirstTile : i < 3 ? scoreForSecondAndThirdTile : scorePerTile
                    };

                    promises.Add(GetAnimationPromise(animationTileData, scorePosition, TileConstants.TILE_SELECTION_ANIMATION_DELAY * i));
                }
            }

            return Promise.All(promises);
        }

        private IPromise GetAnimationPromise(AnimationTileData animationTileData, Vector3 scorePosition, float delay)
        {
            Promise promise = new Promise();
            DOVirtual.DelayedCall(delay, () =>
            {
                animationTileData.Transform.DOKill();
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