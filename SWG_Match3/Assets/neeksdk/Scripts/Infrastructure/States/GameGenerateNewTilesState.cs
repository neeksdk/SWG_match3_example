using System.Collections.Generic;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Infrastructure.Services;
using RSG;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameGenerateNewTilesState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly BoardController _boardController;
        private readonly TileAnimationService _tileAnimationService;

        public GameGenerateNewTilesState(StateMachine stateMachine, BoardController boardController, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _boardController = boardController;
            _tileAnimationService = tileAnimationService;
        }
        
        public void Enter() =>
            GenerateNewTiles();

        public void Exit()
        {
            
        }

        private void GenerateNewTiles()
        {
            if (!_boardController.TryGetAllEmptyTiles(out List<BoardTileData> movedTiles))
            {
                _stateMachine.Enter<GameSelectionState>();
                return;
            }

            MoveTilesOnBoard(movedTiles).Then(() => _stateMachine.Enter<CheckMatchedTilesState>());
        }
        
        private IPromise MoveTilesOnBoard(List<BoardTileData> movedTiles)
        {
            for (int i = movedTiles.Count - 1; i >= 0; i--)
            {
                BoardTileData tileData = movedTiles[i];
                if (tileData.Coords.Row == tileData.Tile.Coords.Row && tileData.Coords.Col == tileData.Tile.Coords.Col)
                {
                    movedTiles.RemoveAt(i);
                }
            }

            return movedTiles.Count == 0 ? Promise.Resolved() : _tileAnimationService.MoveTilesOnBoard(movedTiles).Then(() => MoveTilesOnBoard(movedTiles));
        }
    }
}