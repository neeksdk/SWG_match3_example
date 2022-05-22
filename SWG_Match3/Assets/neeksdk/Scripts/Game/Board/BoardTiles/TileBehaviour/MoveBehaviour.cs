using DG.Tweening;
using neeksdk.Scripts.Constants;
using neeksdk.Scripts.Extensions;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public class MoveBehaviour : IMovable
    {
        private readonly Transform _targetTransform;

        public MoveBehaviour(Transform targetTransform) =>
            _targetTransform = targetTransform;

        public IPromise Move(BoardCoords boardCoords)
        {
            Promise promise = new Promise();
            _targetTransform.DOMove(boardCoords.BoardToVectorCoords(), TileConstants.TILE_MOVE_ANIMATION_DURATION)
                .SetEase(Ease.Linear).OnComplete(promise.Resolve);

            return promise;
        }

        public void Clear()
        {
            if (_targetTransform != null)
            {
                _targetTransform.DOKill();
            }
        }
    }
}