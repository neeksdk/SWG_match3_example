using RSG;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public interface IMovable
    {
        void Move(TileMoveDirections direction);
        IPromise Move(BoardCoords boardCoords);
        void Clear();
    }
}