using DG.Tweening;
using neeksdk.Scripts.Constants;
using neeksdk.Scripts.Extensions;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public class MoveBehaviour : IMovable
    {
        private readonly Transform _transform;

        public MoveBehaviour(Transform transform) =>
            _transform = transform;

        public void Move(TileMoveDirections direction)
        {
            
        }

        public IPromise Move(BoardCoords boardCoords)
        {
            Promise promise = new Promise();
            _transform.DOMove(boardCoords.BoardToVectorCoords(), TileConstants.TILE_MOVE_ANIMATION_DURATION)
                .SetEase(Ease.OutSine).OnComplete(promise.Resolve);

            return promise;
        }

        public void Clear()
        {
            
        }
    }
}