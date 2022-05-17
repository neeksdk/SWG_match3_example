using System.Collections.Generic;
using neeksdk.Scripts.Configs;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEngine;

namespace neeksdk.Scripts.StaticData
{
    public class StaticDataService
    {
        private const string STATIC_DATA_CONFIG_PATH = "Assets/neeksdk/Configs";

        private Dictionary<TileType, TilePrefabData> _tiles;
        private Dictionary<BackgroundType, BackgroundPrefabData> _backgroundTiles;

        public TilePrefabData ForTile(TileType tileType) =>
            _tiles.TryGetValue(tileType, out TilePrefabData tilePrefabData)
                ? tilePrefabData
                : null;

        public BackgroundPrefabData ForBackground(BackgroundType backgroundType) =>
            _backgroundTiles.TryGetValue(backgroundType, out BackgroundPrefabData backgroundPrefabData)
                ? backgroundPrefabData
                : null;

        public void LoadTiles()
        {
            TileConfig[] tileConfigs = Resources.LoadAll<TileConfig>(STATIC_DATA_CONFIG_PATH);
            _tiles = new Dictionary<TileType, TilePrefabData>();
            foreach (TileConfig tileConfig in tileConfigs)
            {
                foreach (TilePrefabData prefabData in tileConfig.Tiles)
                {
                    if (!_tiles.ContainsKey(prefabData.Type))
                    {
                        _tiles.Add(prefabData.Type, prefabData);
                    }
                }
            }
        }

        public void LoadBackgrounds()
        {
            BackgroundConfig[] backgroundConfigs = Resources.LoadAll<BackgroundConfig>(STATIC_DATA_CONFIG_PATH);
            _backgroundTiles = new Dictionary<BackgroundType, BackgroundPrefabData>();
            foreach (BackgroundConfig backgroundConfig in backgroundConfigs)
            {
                foreach (BackgroundPrefabData prefabData in backgroundConfig.Backgrounds)
                {
                    if (!_backgroundTiles.ContainsKey(prefabData.Type))
                    {
                        _backgroundTiles.Add(prefabData.Type, prefabData);
                    }
                }
            }
        }
    }
}