using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public interface ITile
    {
        IPromise ShowUp(bool withAnimation = true);
        void Move(TileMoveDirections moveDirection);
        IPromise Move(BoardCoords boardCoords);
        void Select();
        void Deselect();
        void Cleanup();
        GameObject GameObject { get; }
        TileMonoContainer TileMonoContainer { get; }
        BoardCoords Coords { get; set; }
        TileType TileType { get; }
    }
}