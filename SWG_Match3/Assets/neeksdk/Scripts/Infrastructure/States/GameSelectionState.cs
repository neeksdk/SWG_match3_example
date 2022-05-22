using System.Collections.Generic;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Services;
using UnityEngine;

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
                    _stateMachine.Enter<GameCollectRewardState>();
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

            bool hasFromMatchedTilesHorizontal = _boardController.BoardMatcher.FindMatchedTiles(fromTileData, BoardSearchPattern.Horizontal, out List<BoardTileData> matchingFromTilesHorizontal);
            bool hasFromMatchedTilesVertical = _boardController.BoardMatcher.FindMatchedTiles(fromTileData, BoardSearchPattern.Vertical, out List<BoardTileData> matchingFromTilesVertical);
            bool hasToMatchedTilesHorizontal = _boardController.BoardMatcher.FindMatchedTiles(toTileData, BoardSearchPattern.Horizontal, out List<BoardTileData> matchingToTilesHorizontal);
            bool hasToMatchedTilesVertical = _boardController.BoardMatcher.FindMatchedTiles(toTileData, BoardSearchPattern.Vertical, out List<BoardTileData> matchingToTilesVertical);

            if (hasFromMatchedTilesHorizontal)
            {
                _tileAnimationService.AddCollectAnimationsToQueue(matchingFromTilesHorizontal);
            }

            if (hasFromMatchedTilesVertical)
            {
                _tileAnimationService.AddCollectAnimationsToQueue(matchingFromTilesVertical);
            }
            
            if (hasToMatchedTilesHorizontal)
            {
                _tileAnimationService.AddCollectAnimationsToQueue(matchingToTilesHorizontal);
            }
            
            if (hasToMatchedTilesVertical)
            {
                _tileAnimationService.AddCollectAnimationsToQueue(matchingToTilesVertical);
            }

            return hasFromMatchedTilesHorizontal || hasFromMatchedTilesVertical || hasToMatchedTilesHorizontal || hasToMatchedTilesVertical;
        }


        private void TileSelected(ITile tile) =>
            _gameController.UserSelectTile(tile);
    }
}