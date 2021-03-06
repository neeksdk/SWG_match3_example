using System;
using DG.Tweening;
using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public abstract class BaseTile : ITile
    {
        private readonly TileMonoContainer _tileMonoContainer;
        private const float TILE_ANIMATION_DURATION = 0.025f;
        
        protected IMovable _moveBehaviour;
        protected ICollectable _collectBehaviour;
        protected ISelectable _selectBehaviour;

        protected BaseTile(TileType tileType, TileMonoContainer monoContainer)
        {
            TileType = tileType;
            _tileMonoContainer = monoContainer;
        }
        
        public BoardCoords Coords { get; set; }
        public TileType TileType { get; }
        public GameObject GameObject => _tileMonoContainer.gameObject;
        public TileMonoContainer TileMonoContainer => _tileMonoContainer;

        public IPromise ShowUp(bool withAnimation = true)
        {
            Transform tileTransform = _tileMonoContainer.transform;
            Promise promise = new Promise();
            
            if (!withAnimation)
            {
                tileTransform.localScale = Vector3.one;
                promise.Resolve();
            }
            
            tileTransform.DOScale(Vector3.one, TILE_ANIMATION_DURATION).OnComplete(() => promise.Resolve());
            
            return promise;
        }

        public IPromise Move(BoardCoords boardCoords) =>
            _moveBehaviour.Move(boardCoords).Then(() => Coords = boardCoords);

        public void Select() =>
            _selectBehaviour.Select();

        public void Deselect() =>
            _selectBehaviour.Deselect();

        public IPromise Collect(Vector3 scorePosition, float delayAnimation, Action onComplete) =>
            _collectBehaviour.Collect(scorePosition, delayAnimation, onComplete);

        public void Clear()
        {
            _selectBehaviour.Clear();
            _moveBehaviour.Clear();
            _collectBehaviour.Clear();
        }
    }
}
