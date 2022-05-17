using neeksdk.Scripts.Configs;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.StaticData;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.Factory
{
    public class TileFactory
    {
        private readonly StaticDataService _staticDataService;

        public TileFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public GameObject CreateTile(TileType tileType, Transform parent)
        {
            TilePrefabData tilePrefabData = _staticDataService.ForTile(tileType);
            
            return GameObject.Instantiate(tilePrefabData.TilePrefab, parent);
        }

        public GameObject CreateBackgroundTile(BackgroundType backgroundType, Transform parent)
        {
            BackgroundPrefabData backgroundPrefabData = _staticDataService.ForBackground(backgroundType);

            return GameObject.Instantiate(backgroundPrefabData.TilePrefab, parent);
        }
    }
}