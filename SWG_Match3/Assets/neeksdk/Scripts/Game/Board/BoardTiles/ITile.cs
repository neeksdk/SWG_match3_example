using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public interface ITile
    {
        void Move(TileMoveDirections moveDirection);
    }
}