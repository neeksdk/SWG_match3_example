using UnityEngine;

namespace neeksdk.Scripts.Configs
{
    [CreateAssetMenu(fileName = "TileConfig", menuName = "Configs/Tile config")]
    public class TileConfig : ScriptableObject
    {
        public TilePrefabData[] Tiles;
    }
}
