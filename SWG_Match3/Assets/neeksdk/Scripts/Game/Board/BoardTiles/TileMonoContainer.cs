using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class TileMonoContainer : MonoBehaviour
    {
        private ITile _tile;

        public ITile Tile => _tile;

        public void SetupTile(ITile tile) =>
            _tile = tile;
    }
}
