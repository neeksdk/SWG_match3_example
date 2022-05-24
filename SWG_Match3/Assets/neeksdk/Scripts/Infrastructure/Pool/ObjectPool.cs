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
        private readonly Dictionary<BackgroundType, Queue<IBackground>> _backgroundPool = new Dictionary<BackgroundType, Queue<IBackground>>();

        private TileFactory _tileFactory;

        public static ObjectPool Instance = null;

        public void InitializePool(int initialSize, TileFactory tileFactory, params TileType[] tileTypesForLevel)
        {
            _tileFactory = tileFactory;
            int halfSize = initialSize / 2;
            int tileOfEachTypeSize = initialSize / tileTypesForLevel.Length + 5;
            int emptyTiles = 3;
            
            for (int i = 0; i < halfSize; i++)
            {
                AddBackgroundToPool(BackgroundType.Standard);
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

            ITile tile = _tilePool[tileType].Dequeue();
            GameObject go = tile.GameObject;
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;
            
            return tile;
        }

        public void Recycle(TileMonoContainer tile)
        {
            if (tile == null)
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

            IBackground background = _backgroundPool[backgroundType].Dequeue();
            background.GameObject.transform.SetParent(parent);
            background.GameObject.transform.position = position;
            background.GameObject.transform.rotation = rotation;

            return background;
        }

        public void Recycle(BackgroundMonoContainer background)
        {
            if (background == null)
            {
                return;
            } 
            
            ResetPrefabGameObject(background.gameObject);
            ReturnToPool(background);
        } 

        private void Awake()
        {
            if (Instance == this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
        
        private void AddBackgroundToPool(BackgroundType backgroundType)
        {
            CheckBackgroundPoolDataAvailability(backgroundType);
            
            IBackground background = _tileFactory.CreateStandardBackgroundTile(backgroundType, transform);
            background.GameObject.SetActive(false);
            _backgroundPool[backgroundType].Enqueue(background);
        }

        private void AddTileToPool(TileType tileType)
        {
            CheckTilePoolDataAvailability(tileType);
            
            ITile tile = _tileFactory.CreateStandardTile(tileType, transform);
            tile.GameObject.SetActive(false);
            _tilePool[tileType].Enqueue(tile);
        }

        private void ReturnToPool(TileMonoContainer tile)
        {
            if (tile.Tile == null || !_tilePool.ContainsKey(tile.Tile.TileType))
            {
                Destroy(tile.gameObject);
                return;
            }
            
            _tilePool[tile.Tile.TileType].Enqueue(tile.Tile);
        }

        private void ReturnToPool(BackgroundMonoContainer background)
        {
            if (background == null || !_backgroundPool.ContainsKey(background.Background.BackgroundType))
            {
                Destroy(background.gameObject);
                return;
            }
            
            _backgroundPool[background.Background.BackgroundType].Enqueue(background.Background);
        }
        
        private void CheckBackgroundPoolDataAvailability(BackgroundType backgroundType)
        {
            if (!_backgroundPool.ContainsKey(backgroundType))
                _backgroundPool.Add(backgroundType, new Queue<IBackground>());
        }

        private void CheckTilePoolDataAvailability(TileType tileType)
        {
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