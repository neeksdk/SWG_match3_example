using System.Collections.Generic;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Infrastructure.Services;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class CheckMatchedTilesState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly BoardController _boardController;
        private readonly TileAnimationService _tileAnimationService;

        public CheckMatchedTilesState(StateMachine stateMachine, BoardController boardController, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _boardController = boardController;
            _tileAnimationService = tileAnimationService;
        }
        
        public void Enter()
        {
            if (!_boardController.BoardMatcher.TryToFindMatchesOnAllBoard(out List<BoardTileData> tileData))
            {
                _stateMachine.Enter<GameSelectionState>();
                return;
            }

            if (_tileAnimationService.HasAnimations())
            {
                _tileAnimationService.ClearCollectAnimationQueue();
            }
            
            _tileAnimationService.AddCollectAnimationsToQueue(tileData);
            _stateMachine.Enter<GameCollectRewardState>();
        }

        public void Exit()
        {
            
        }
    }
}