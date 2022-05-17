using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public interface ITile
    {
        void Move(TileMoveDirections moveDirection);
        GameObject GameObject { get; }
        TileType TileType { get; }
    }
}