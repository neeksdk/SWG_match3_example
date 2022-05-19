using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board.BoardTiles;
using RSG;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameSelectionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameController _gameController;

        public GameSelectionState(StateMachine stateMachine, GameController gameController)
        {
            _stateMachine = stateMachine;
            _gameController = gameController;
        }
        
        public void Enter()
        {
            _gameController.ClearSelectionData();
            _gameController.OnSwapTiles += SwapTiles;
            TileMonoContainer.OnTileSelected += TileSelected;
        }

        public void Exit() =>
            TileMonoContainer.OnTileSelected -= TileSelected;

        private void SwapTiles(IPromise swapTilesPromise)
        {
            TileMonoContainer.OnTileSelected -= TileSelected;
            swapTilesPromise.Then(() => _stateMachine.Enter<GameAnimationState>());
        }

        private void TileSelected(ITile tile) =>
            _gameController.UserSelectTile(tile);
    }
}