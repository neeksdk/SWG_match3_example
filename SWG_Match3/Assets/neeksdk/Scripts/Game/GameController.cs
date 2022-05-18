using neeksdk.Scripts.Game.Board;

namespace neeksdk.Scripts.Game
{
    public class GameController
    {
        private readonly BoardController _boardController;

        public GameController(BoardController boardController)
        {
            _boardController = boardController;
        }
    }
}