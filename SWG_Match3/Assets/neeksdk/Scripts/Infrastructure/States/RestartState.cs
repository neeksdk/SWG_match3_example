using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Game.GameUIView;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class RestartState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly BoardController _boardController;
        private readonly GameUiView _gameUiView;

        public RestartState(StateMachine stateMachine, BoardController boardController, GameUiView gameUiView)
        {
            _stateMachine = stateMachine;
            _boardController = boardController;
            _gameUiView = gameUiView;
        }
        
        public void Enter()
        {
            _boardController.ClearBoard();
            _gameUiView.ResetScorePoints();
            BoardData levelData = GetLevelData();
            _boardController.SetupLevel(levelData).Then(() => _stateMachine.Enter<GameSelectionState>());
        }

        public void Exit()
        {
            
        }
        
        private BoardData GetLevelData() =>
            new BoardData()
            {
                Rows = 6,
                Cols = 6,
                EmptyTiles = 3,
                TileTypes = new TileType[] {TileType.Fire, TileType.Leaf, TileType.Lighting, TileType.Water}
            };
    }
}