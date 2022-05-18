using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Factory;
using neeksdk.Scripts.Infrastructure.Pool;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class LoadingState : IState
    {
        private readonly ObjectPool _objectPool;
        private readonly TileFactory _tileFactory;
        private readonly BoardController _boardController;

        public LoadingState(ObjectPool objectPool, TileFactory tileFactory, BoardController boardController)
        {
            _objectPool = objectPool;
            _tileFactory = tileFactory;
            _boardController = boardController;
        }
        
        public void Enter()
        {
            TileType[] tileTypes = new TileType[] {TileType.Fire, TileType.Leaf, TileType.Lighting, TileType.Water};
            _objectPool.InitializePool(36, _tileFactory, tileTypes);
            _boardController.SetupLevel(new BoardData()
            {
                Rows = 6, 
                Cols = 6, 
                EmptyTiles = 3, 
                TileTypes = tileTypes
            });
        }

        public void Exit()
        {
            
        }
    }
}