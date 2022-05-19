using System;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using RSG;

namespace neeksdk.Scripts.Game
{
    public class GameController
    {
        private readonly BoardController _boardController;
        
        private ITile _firstTileSelected;

        public Action<IPromise> OnSwapTiles;

        public GameController(BoardController boardController) =>
            _boardController = boardController;

        public void ClearSelectionData() =>
            _firstTileSelected = null;

        public void UserSelectTile(ITile tile)
        {
            if (_firstTileSelected == null)
            {
                _firstTileSelected = tile;
                _firstTileSelected.Select();
                return;
            }

            if (_firstTileSelected == tile)
            {
                return;
            }

            if (!_firstTileSelected.IsNeighbourTile(tile))
            {
                _firstTileSelected.Deselect();
                _firstTileSelected = tile;
                _firstTileSelected.Select();
                return;
            }

            OnSwapTiles(_boardController.SwapTiles(_firstTileSelected, tile));
        }
    }   
}