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

        public static bool Recycle(this ITile tile)
        {
            if (ObjectPool.Instance == null)
            {
                return false;
            }
            
            tile.Clear();
            ObjectPool.Instance.Recycle(tile.TileMonoContainer);
            return true;
        }
        
        public static bool Recycle(this IBackground background)
        {
            if (ObjectPool.Instance == null)
            {
                return false;
            }
            
            ObjectPool.Instance.Recycle(background.BackgroundMonoContainer);
            return true;
        }
    }
}