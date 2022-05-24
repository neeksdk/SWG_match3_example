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

        public ITile CreateStandardTile(TileType tileType, Transform parent)
        {
            TilePrefabData tilePrefabData = _staticDataService.ForTile(tileType);
            TileMonoContainer tileMonoContainer = Object.Instantiate(tilePrefabData.TilePrefab, parent);
            ITile tile = GetTile(tileType, tileMonoContainer);
            tileMonoContainer.SetupTile(tile);

            return tile;
        }

        public IBackground CreateStandardBackgroundTile(BackgroundType backgroundType, Transform parent)
        {
            BackgroundPrefabData backgroundPrefabData = _staticDataService.ForBackground(backgroundType);
            BackgroundMonoContainer backgroundMonoContainer = Object.Instantiate(backgroundPrefabData.TilePrefab, parent);
            backgroundMonoContainer. SetupBackground(new StandardBackground(backgroundType, backgroundMonoContainer));

            return backgroundMonoContainer.Background;
        }

        private ITile GetTile(TileType tileType, TileMonoContainer tileMonoContainer) =>
            new StandardTile(tileType, tileMonoContainer);
    }
}