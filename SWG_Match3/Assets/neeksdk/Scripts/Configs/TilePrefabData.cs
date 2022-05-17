using System;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEngine;

namespace neeksdk.Scripts.Configs
{
    [Serializable]
    public class TilePrefabData
    {
        public string Name;
        public TileType Type;
        public GameObject TilePrefab;
    }
}