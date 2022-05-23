namespace neeksdk.Scripts.Infrastructure.States
{
    public class RestartState : IState
    {
        private readonly StateMachine _stateMachine;

        public RestartState(StateMachine stateMachine)
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