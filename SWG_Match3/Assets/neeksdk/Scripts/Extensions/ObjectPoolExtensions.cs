using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Pool;
using UnityEngine;

namespace neeksdk.Scripts.Extensions
{
    public static class ObjectPoolExtensions
    {
        public static bool Spawn(this BackgroundType backgroundType, Transform transform, out IBackground background, Vector3 position = default, Quaternion rotation = default)
        {
            background = null;
            if (ObjectPool.Instance == null)
            {
                return false;
            }

            background = ObjectPool.Instance.Spawn(backgroundType, transform, position, rotation);
            return true; 
        }
        
        public static bool Spawn(this TileType tileType, Transform transform, out ITile tile, Vector3 position = default, Quaternion rotation = default)
        {
            tile = null;
            if (ObjectPool.Instance == null)
            {
                return false;
            }

            tile = ObjectPool.Instance.Spawn(tileType, transform, position, rotation);
            return true; 
        }
    }
}