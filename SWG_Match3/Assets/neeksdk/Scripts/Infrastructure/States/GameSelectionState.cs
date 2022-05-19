using System.Collections.Generic;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Services;
using RSG;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class GameSelectionState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly GameController _gameController;
        private readonly BoardController _boardController;
        private readonly TileAnimationService _tileAnimationService;

        public GameSelectionState(StateMachine stateMachine, GameController gameController, BoardController boardController, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _gameController = gameController;
            _boardController = boardController;
            _tileAnimationService = tileAnimationService;
        }
        
        public void Enter()
        {
            _gameController.ClearSelectionData();
            Subscribe();
        }

        public void Exit()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _gameController.OnSwapTiles += SwapTiles;
            TileMonoContainer.OnTileSelected += TileSelected;
        }

        private void Unsubscribe()
        {
            _gameController.ClearSelectionData();
            _gameController.OnSwapTiles -= SwapTiles;
            TileMonoContainer.OnTileSelected -= TileSelected;
        }
        
        private void SwapTiles(ITile fromTile, ITile toTile)
        {
            Unsubscribe();
            _boardController.SwapTiles(fromTile, toTile).Then(() =>
            {
                if (CheckMatchedTiles(fromTile, toTile))
                {
                    _stateMachine.Enter<GameAnimationState>();
                }
                else
                {
                    Subscribe();
                }
            });
        }

        private bool CheckMatchedTiles(ITile fromTile, ITile toTile)
        {
            BoardTileData fromTileData = _boardController.GetBoardTileData(fromTile);
            BoardTileData toTileData = _boardController.GetBoardTileData(toTile);

            bool hasFromMatchedTiles = _boardController.BoardMatcher.FindMatchedTiles(fromTileData, BoardSearchPattern.Both, out List<BoardTileData> intersectingFromTiles);
            bool hasToMatchedTiles = _boardController.BoardMatcher.FindMatchedTiles(toTileData, BoardSearchPattern.Both, out List<BoardTileData> intersectingToTiles);

            if (hasFromMatchedTiles)
            {
                _tileAnimationService.AddAnimationsToQueue(intersectingFromTiles);
            }

            if (hasToMatchedTiles)
            {
                _tileAnimationService.AddAnimationsToQueue(intersectingToTiles);
            }

            return hasFromMatchedTiles || hasToMatchedTiles;
        }


        private void TileSelected(ITile tile) =>
            _gameController.UserSelectTile(tile);
    }
}