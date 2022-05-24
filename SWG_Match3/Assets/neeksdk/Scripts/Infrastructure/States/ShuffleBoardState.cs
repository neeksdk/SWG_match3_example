using System.Collections.Generic;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardControl;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Services;

namespace neeksdk.Scripts.Infrastructure.States
{
    public class ShuffleBoardState : IState
    {
        private readonly StateMachine _stateMachine;
        private readonly BoardController _boardController;
        private readonly TileAnimationService _tileAnimationService;

        public ShuffleBoardState(StateMachine stateMachine, BoardController boardController, TileAnimationService tileAnimationService)
        {
            _stateMachine = stateMachine;
            _boardController = boardController;
            _tileAnimationService = tileAnimationService;
        }

        public void Enter()
        {
            _boardController.ShuffleBoard();
            if (!TryToGetMovedTilesData(out List<BoardTileData> tilesNeedToBeMoved))
            {
                _stateMachine.Enter<GameSelectionState>();
                return;
            }

            _tileAnimationService.PlayShuffleTileAnimations(tilesNeedToBeMoved).Then(() => _stateMachine.Enter<CheckMatchedTilesState>());
        }

        public void Exit()
        {
            
        }

        private bool TryToGetMovedTilesData(out List<BoardTileData> tilesNeedToBeMoved)
        { 
            tilesNeedToBeMoved = new List<BoardTileData>();
            foreach (BoardTileData boardTileData in _boardController.BoardTileData)
            {
                if (boardTileData.BackgroundType == BackgroundType.Empty || boardTileData.Tile == null)
                {
                    continue;
                }

                ITile tile = boardTileData.Tile;
                if (tile.Coords.Row != boardTileData.Coords.Row || tile.Coords.Col != boardTileData.Coords.Col)
                {
                    tilesNeedToBeMoved.Add(boardTileData);
                }
            }

            return tilesNeedToBeMoved.Count > 0;
        }
    }
}