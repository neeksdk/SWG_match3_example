using neeksdk.Scripts.Infrastructure.Services;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameCollectRewardState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly TileAnimationService _tileAnimationService;

        public GameCollectRewardState(StateMachine stateMachine, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _tileAnimationService = tileAnimationService;
        }
        
        public void Enter()
        {
            if (!_tileAnimationService.HasAnimations())
            {
                _stateMachine.Enter<GameSelectionState>();
                return;
            }

            _tileAnimationService.PlayCollectTileAnimations().Then(() => _stateMachine.Enter<GameGenerateNewTilesState>());
        }

        public void Exit()
        {
            
        }
    }
}