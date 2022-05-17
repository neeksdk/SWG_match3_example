using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public abstract class BaseTile : ITile
    {
        protected IMovable _moveBehaviour;
        protected ICollectable _collectBehaviour;

        public void Move(TileMoveDirections moveDirection) =>
            _moveBehaviour.Move(moveDirection);

        public void Collect() =>
            _collectBehaviour.Collect();
    }
}
