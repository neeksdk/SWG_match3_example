using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Factory;
using neeksdk.Scripts.Infrastructure.Pool;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class LoadingState : IState
    {
        private readonly ObjectPool _objectPool;
        private readonly TileFactory _tileFactory;

        public LoadingState(ObjectPool objectPool, TileFactory tileFactory)
        {
            _objectPool = objectPool;
            _tileFactory = tileFactory;
        }
        
        public void Enter()
        {
            _objectPool.InitializePool(36, _tileFactory, TileType.Fire, TileType.Leaf, TileType.Water, TileType.Lighting);
        }

        public void Exit()
        {
            
        }
    }
}