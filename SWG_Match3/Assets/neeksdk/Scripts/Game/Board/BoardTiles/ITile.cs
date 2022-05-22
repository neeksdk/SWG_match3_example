using neeksdk.Scripts.Game.Board.BoardTiles.TileBehaviour;
using RSG;
using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardTiles
{
    public interface ITile : IMovable, ICollectable, ISelectable
    {
        IPromise ShowUp(bool withAnimation = true);
        GameObject GameObject { get; }
        TileMonoContainer TileMonoContainer { get; }
        BoardCoords Coords { get; set; }
        TileType TileType { get; }
    }
}