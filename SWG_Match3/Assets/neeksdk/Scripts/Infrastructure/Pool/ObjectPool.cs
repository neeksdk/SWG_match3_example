using System.Collections.Generic;
using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;
using neeksdk.Scripts.Infrastructure.Factory;
using UnityEngine;

namespace neeksdk.Scripts.Infrastructure.Pool
{
    public class ObjectPool : MonoBehaviour
    {
        private Dictionary<TileType, Queue<ITile>> _tilePool = new Dictionary<TileType, Queue<ITile>>();
        private Dictionary<TileType, Queue<GameObject>> _tileGoPool = new Dictionary<TileType, Queue<GameObject>>();

        private Dictionary<BackgroundType, Queue<IBackground>> _backgroundPool = new Dictionary<BackgroundType, Queue<IBackground>>();
        private Dictionary<BackgroundType, Queue<GameObject>> _backgroundGoPool = new Dictionary<BackgroundType, Queue<GameObject>>();

        private TileFactory _tileFactory;
        
        public void InitializePool(int initialSize, TileFactory tileFactory)
        {
            _tileFactory = tileFactory;
            int halfSize = initialSize / 2;
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
        }

        public ITile Spawn(TileType tileType, Transform parent = null, Vector3 position = default, Quaternion rotation = default)
        {
            if (!_tilePool.ContainsKey(tileType) || _tilePool[tileType].Count == 0) AddTileToPool(tileType);

            GameObject go = _tileGoPool[tileType].Dequeue();
            go.transform.SetParent(parent);
            go.transform.position = position;
            go.transform.rotation = rotation;

            return _tilePool[tileType].Dequeue();
        }

        public void Recycle(ITile tile)
        {
            if (tile == null || tile.GameObject == null)
            {
                return;
            } 
            
            GameObject go = tile.GameObject;
            go.SetActive(false);
            go.transform.parent = transform;
            go.transform.position = default;
            go.transform.rotation = default;
            ReturnToPool(tile);
        } 
        
        public void Recycle(IBackground background)
        {
            if (background == null || background.GameObject == null)
            {
                return;
            } 
            
            GameObject go = background.GameObject;
            go.SetActive(false);
            go.transform.parent = transform;
            go.transform.position = default;
            go.transform.rotation = default;
            ReturnToPool(background);
        } 

        private void AddBackgroundToPool(BackgroundType backgroundType)
        {
            GameObject backgroundGo = _tileFactory.CreateBackgroundTile(BackgroundType.StandardA, transform);
            backgroundGo.SetActive(false);
            
            if (!_backgroundGoPool.ContainsKey(backgroundType))
                _backgroundGoPool.Add(backgroundType, new Queue<GameObject>());
            
            _backgroundGoPool[backgroundType].Enqueue(backgroundGo);
            
            
            IBackground background = backgroundGo.GetComponent<IBackground>();
            
            if (!_backgroundPool.ContainsKey(backgroundType))
                _backgroundPool.Add(backgroundType, new Queue<IBackground>());
            
            _backgroundPool[backgroundType].Enqueue(background);
        }

        private void AddTileToPool(TileType tileType)
        {
            GameObject tileGo = _tileFactory.CreateTile(tileType, transform);
            tileGo.SetActive(false);
            
            if (!_tileGoPool.ContainsKey(tileType))
                _tileGoPool.Add(tileType, new Queue<GameObject>());
            
            _tileGoPool[tileType].Enqueue(tileGo);


            ITile tile = tileGo.GetComponent<ITile>();
            
            if (!_tilePool.ContainsKey(tileType))
                _tilePool.Add(tileType, new Queue<ITile>());
            
            _tilePool[tileType].Enqueue(tile);
        }

        private void ReturnToPool(ITile tile)
        {
            if (!_tileGoPool.ContainsKey(tile.TileType) || !_tilePool.ContainsKey(tile.TileType))
            {
                Destroy(tile.GameObject);
                return;
            }
            
            _tileGoPool[tile.TileType].Enqueue(tile.GameObject);
            _tilePool[tile.TileType].Enqueue(tile);
        }

        private void ReturnToPool(IBackground background)
        {
            if (!_backgroundGoPool.ContainsKey(background.BackgroundType) || !_backgroundPool.ContainsKey(background.BackgroundType))
            {
                Destroy(background.GameObject);
                return;
            }
            
            _backgroundGoPool[background.BackgroundType].Enqueue(background.GameObject);
            _backgroundPool[background.BackgroundType].Enqueue(background);
        }
    }
}