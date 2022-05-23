using System;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public class TileMonoContainer : MonoBehaviour
    {
        public static Action<ITile> OnTileSelected;

        public ITile Tile { get; private set; }

        public void SetupTile(ITile tile) =>
            Tile = tile;

        private void OnMouseDown()
        {
            if (Tile == null)
            {
                return;
            }
            
            OnTileSelected?.Invoke(Tile);
        }
    }
}
