using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public abstract class BaseTile : MonoBehaviour, ITile
    {
        protected TileType _tileType;
        protected IMovable _moveBehaviour;
        protected ICollectable _collectBehaviour;

        public GameObject GameObject => gameObject;
        public TileType TileType => _tileType; 
        
        public void Move(TileMoveDirections moveDirection) =>
            _moveBehaviour.Move(moveDirection);

        public void Collect() =>
            _collectBehaviour.Collect();
    }
}
