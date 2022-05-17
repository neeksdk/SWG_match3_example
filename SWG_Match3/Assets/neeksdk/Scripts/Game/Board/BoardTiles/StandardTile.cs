using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class StandardTile : BaseTile
    {
        public StandardTile()
        {
            _moveBehaviour = new MoveBehaviour();
            _collectBehaviour = new CollectBehaviour();
        }
    }
}