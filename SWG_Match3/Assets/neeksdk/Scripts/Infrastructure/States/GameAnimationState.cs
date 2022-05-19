using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Infrastructure.Services;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameAnimationState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly BoardController _boardController;
        private readonly TileAnimationService _tileAnimationService;

        public GameAnimationState(StateMachine stateMachine, BoardController boardController, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _boardController = boardController;
            _tileAnimationService = tileAnimationService;
        }
        
        public void Enter()
        {
            if (!_tileAnimationService.HasAnimations())
            {
                _stateMachine.Enter<GameSelectionState>();
                return;
            }   
            
            
        }

        public void Exit()
        {
            
        }
    }
}