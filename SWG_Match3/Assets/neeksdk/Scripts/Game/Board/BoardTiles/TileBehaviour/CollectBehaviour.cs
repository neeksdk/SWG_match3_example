using System;
using DG.Tweening;
using neeksdk.Scripts.Constants;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public class CollectBehaviour : ICollectable
    {
        private Sequence _collectTween;
        private readonly Transform _targetTargetTransform;

        public CollectBehaviour(Transform targetTransform) =>
            _targetTargetTransform = targetTransform;

        public IPromise Collect(Vector3 scorePosition, Action onComplete)
        {
            if (_collectTween != null && _collectTween.IsPlaying())
            {
                return Promise.Resolved();
            }
            
            Promise promise = new Promise();
            _collectTween = DOTween.Sequence();
            _collectTween.Append(_targetTargetTransform.DOMove(scorePosition, TileConstants.TILE_MOVE_ANIMATION_DURATION)
                .SetEase(Ease.OutSine)
                .OnComplete(() =>
                {
                    onComplete?.Invoke();
                }));
            _collectTween.Append(_targetTargetTransform.DOScale(Vector3.zero, TileConstants.TILE_MOVE_ANIMATION_DURATION)
                .SetEase(Ease.OutSine));
            _collectTween.Play().OnComplete(() =>
            {
                promise.Resolve();
            });

            return promise;
        }

        public void Clear()
        {
            if (_collectTween == null || !_collectTween.IsPlaying())
            {
                return;
            }
            
            _collectTween.Kill(true);
        }
    }
}