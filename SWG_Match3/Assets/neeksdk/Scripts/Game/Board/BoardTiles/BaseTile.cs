using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public abstract class BaseTile : ITile
    {
        public TileType TileType { get; }
        private readonly TileMonoContainer _tileMonoContainer;
        
        protected IMovable _moveBehaviour;
        protected ICollectable _collectBehaviour;

        protected BaseTile(TileType tileType, TileMonoContainer monoContainer)
        {
            TileType = tileType;
            _tileMonoContainer = monoContainer;
        }
        
        public GameObject GameObject => _tileMonoContainer.gameObject;

        public void Move(TileMoveDirections moveDirection) =>
            _moveBehaviour.Move(moveDirection);

        public void Collect() =>
            _collectBehaviour.Collect();
    }
}
