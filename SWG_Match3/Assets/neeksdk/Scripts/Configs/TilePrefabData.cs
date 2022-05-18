using System;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Configs
{
    [Serializable]
    public class TilePrefabData
    {
        public string Name;
        public TileType Type;
        public TileMonoContainer TilePrefab;
    }
}