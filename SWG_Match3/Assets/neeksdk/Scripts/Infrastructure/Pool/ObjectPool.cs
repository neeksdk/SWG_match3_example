using System.Collections.Generic;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Factory;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        private readonly Dictionary<TileType, Queue<ITile>> _tilePool = new Dictionary<TileType, Queue<ITile>>();
        private readonly Dictionary<TileType, Queue<TileMonoContainer>> _tileGoPool = new Dictionary<TileType, Queue<TileMonoContainer>>();

        private readonly Dictionary<BackgroundType, Queue<IBackground>> _backgroundPool = new Dictionary<BackgroundType, Queue<IBackground>>();
        private readonly Dictionary<BackgroundType, Queue<BackgroundMonoContainer>> _backgroundGoPool = new Dictionary<BackgroundType, Queue<BackgroundMonoContainer>>();

        private TileFactory _tileFactory;
        
        public void InitializePool(int initialSize, TileFactory tileFactory, params TileType[] tileTypesForLevel)
        {
            _tileFactory = tileFactory;
            int halfSize = initialSize / 2;
            int tileOfEachTypeSize = initialSize / tileTypesForLevel.Length + 1;
            int emptyTiles = 3;
            
            for (int i = 0; i < halfSize; i++)
            {
                AddBackgroundToPool(BackgroundType.StandardA);
                AddBackgroundToPool(BackgroundType.StandardB);
            }

            for (int i = 0; i < emptyTiles; i++)
            {
                AddBackgroundToPool(BackgroundType.Empty);
            }

            for (int i = 0; i < tileOfEachTypeSize; i++)
            {
                foreach (TileType t in tileTypesForLevel)
                {
                    AddTileToPool(t);
                }
            }
        }

        public ITile Spawn(TileType tileType, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
        {
            if (!_tilePool.ContainsKey(tileType) || _tilePool[tileType].Count == 0)
            {
                AddTileToPool(tileType);
            }

            GameObject go = _tileGoPool[tileType].Dequeue().gameObject;
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;

            return _tilePool[tileType].Dequeue();
        }

        public void Recycle(TileMonoContainer tile)
        {
            if (tile == null || tile.gameObject == null)
            {
                return;
            } 
            
            ResetPrefabGameObject(tile.gameObject);
            ReturnToPool(tile);
        }
        
        public IBackground Spawn(BackgroundType backgroundType, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
        {
            if (!_backgroundPool.ContainsKey(backgroundType) || _backgroundPool[backgroundType].Count == 0)
            {
                AddBackgroundToPool(backgroundType);
            }

            GameObject go = _backgroundGoPool[backgroundType].Dequeue().gameObject;
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;

            return _backgroundPool[backgroundType].Dequeue();
        }

        public void Recycle(BackgroundMonoContainer background)
        {
            if (background == null || background.gameObject == null)
            {
                return;
            } 
            
            ResetPrefabGameObject(background.gameObject);
            ReturnToPool(background);
        } 

        private void AddBackgroundToPool(BackgroundType backgroundType)
        {
            CheckBackgroundPoolDataAvailability(backgroundType);
            
            BackgroundMonoContainer backgroundMonoContainer = _tileFactory.CreateStandardBackgroundTile(BackgroundType.StandardA, transform);
            backgroundMonoContainer.gameObject.SetActive(false);
            _backgroundGoPool[backgroundType].Enqueue(backgroundMonoContainer);
            _backgroundPool[backgroundType].Enqueue(backgroundMonoContainer.Background);
        }

        private void AddTileToPool(TileType tileType)
        {
            CheckTilePoolDataAvailability(tileType);
            
            TileMonoContainer tileGo = _tileFactory.CreateStandardTile(tileType, transform);
            tileGo.gameObject.SetActive(false);
            _tileGoPool[tileType].Enqueue(tileGo);
            _tilePool[tileType].Enqueue(tileGo.Tile);
        }

        private void ReturnToPool(TileMonoContainer tile)
        {
            if (!_tileGoPool.ContainsKey(tile.Tile.TileType) || !_tilePool.ContainsKey(tile.Tile.TileType))
            {
                Destroy(tile.gameObject);
                return;
            }
            
            _tileGoPool[tile.Tile.TileType].Enqueue(tile);
            _tilePool[tile.Tile.TileType].Enqueue(tile.Tile);
        }

        private void ReturnToPool(BackgroundMonoContainer background)
        {
            if (!_backgroundGoPool.ContainsKey(background.Background.BackgroundType) || !_backgroundPool.ContainsKey(background.Background.BackgroundType))
            {
                Destroy(background.gameObject);
                return;
            }
            
            _backgroundGoPool[background.Background.BackgroundType].Enqueue(background);
            _backgroundPool[background.Background.BackgroundType].Enqueue(background.Background);
        }
        
        private void CheckBackgroundPoolDataAvailability(BackgroundType backgroundType)
        {
            if (!_backgroundGoPool.ContainsKey(backgroundType))
                _backgroundGoPool.Add(backgroundType, new Queue<BackgroundMonoContainer>());

            if (!_backgroundPool.ContainsKey(backgroundType))
                _backgroundPool.Add(backgroundType, new Queue<IBackground>());
        }

        private void CheckTilePoolDataAvailability(TileType tileType)
        {
            if (!_tileGoPool.ContainsKey(tileType))
                _tileGoPool.Add(tileType, new Queue<TileMonoContainer>());

            if (!_tilePool.ContainsKey(tileType))
                _tilePool.Add(tileType, new Queue<ITile>());
        }
        
        private void ResetPrefabGameObject(GameObject go)
        {
            go.SetActive(false);
            go.transform.parent = transform;
            go.transform.position = default;
            go.transform.rotation = default;
        }
    }
}