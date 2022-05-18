using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class StandardTile : BaseTile
    {
        public StandardTile(TileType tileType, TileMonoContainer monoContainer) : base(tileType, monoContainer)
        {
            _moveBehaviour = new MoveBehaviour();
            _collectBehaviour = new CollectBehaviour();
        }
    }
}