using System;
using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board.BoardControl;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game
{
    public class GameController
    {
        private readonly BoardController _boardController;
        
        private ITile _firstTileSelected;

        public Action<ITile, ITile> OnSwapTiles;

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

            OnSwapTiles(_firstTileSelected, tile);
        }
    }   
}