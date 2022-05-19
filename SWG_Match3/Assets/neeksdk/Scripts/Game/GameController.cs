using neeksdk.Scripts.Extensions;
using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game
{
    public class GameController
    {
        private readonly BoardController _boardController;
        
        private ITile _firstTileSelected;
        private ITile _secondTileSelected;

        public GameController(BoardController boardController)
        {
            _boardController = boardController;
        }

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
            
            //todo: swap tiles
        }
    }   
}