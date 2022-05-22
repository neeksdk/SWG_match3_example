using RSG;

namespace neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour
{
    public interface IMovable : IBehaviourClearable
    {
        IPromise Move(BoardCoords boardCoords);
    }
}