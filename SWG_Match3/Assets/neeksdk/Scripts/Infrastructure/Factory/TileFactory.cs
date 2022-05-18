using System;
using neeksdk.Scripts.Configs;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.StaticData;
using UnityEngine;
using Object = UnityEngine.Object;

namespace neeksdk.Scripts.Infrastructure.Factory
{
    public class TileFactory
    {
        private readonly StaticDataService _staticDataService;

        public TileFactory(StaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }

        public TileMonoContainer CreateStandardTile(TileType tileType, Transform parent)
        {
            TilePrefabData tilePrefabData = _staticDataService.ForTile(tileType);
            TileMonoContainer tileMonoContainer = Object.Instantiate(tilePrefabData.TilePrefab, parent);
            tileMonoContainer.SetupTile(new StandardTile(tileType, tileMonoContainer));
            
            return tileMonoContainer;
        }

        public BackgroundMonoContainer CreateStandardBackgroundTile(BackgroundType backgroundType, Transform parent)
        {
            BackgroundPrefabData backgroundPrefabData = _staticDataService.ForBackground(backgroundType);
            BackgroundMonoContainer backgroundMonoContainer = Object.Instantiate(backgroundPrefabData.TilePrefab, parent);
            backgroundMonoContainer. SetupBackground(new StandardBackground(backgroundType, backgroundMonoContainer));

            return backgroundMonoContainer;
        }
    }
}