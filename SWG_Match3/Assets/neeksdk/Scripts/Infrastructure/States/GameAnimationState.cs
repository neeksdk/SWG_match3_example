namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameAnimationState : IState
    {
        private readonly StateMachine _stateMachine;

        public GameAnimationState(StateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }
        
        public void Enter()
        {
            
        }

        public void Exit()
        {
            
        }
    }
}