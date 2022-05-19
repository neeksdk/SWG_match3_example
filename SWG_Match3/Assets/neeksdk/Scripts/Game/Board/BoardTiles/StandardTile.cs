using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class StandardTile : BaseTile
    {
        public StandardTile(TileType tileType, TileMonoContainer monoContainer) : base(tileType, monoContainer)
        {
            Transform transform = monoContainer.transform;
            
            _moveBehaviour = new MoveBehaviour(transform);
            _collectBehaviour = new CollectBehaviour();
            _selectBehaviour = new SelectBehaviour(transform);
        }
    }
}