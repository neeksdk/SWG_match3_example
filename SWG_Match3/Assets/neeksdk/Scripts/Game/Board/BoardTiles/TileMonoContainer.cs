using System;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class TileMonoContainer : MonoBehaviour
    {
        private ITile _tile;

        public static Action<ITile> OnTileSelected;

        public ITile Tile => _tile;

        public void SetupTile(ITile tile) =>
            _tile = tile;

        private void OnMouseDown()
        {
            if (_tile == null)
            {
                return;
            }
            
            OnTileSelected?.Invoke(_tile);
        }
    }
}
