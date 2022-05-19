using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board.BoardTiles;

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
            TileMonoContainer.OnTileSelected += TileSelected;
        }

        public void Exit()
        {
            TileMonoContainer.OnTileSelected -= TileSelected;
        }

        private void TileSelected(ITile tile)
        {
            _gameController.UserSelectTile(tile);
        }
    }
}