using System.Collections.Generic;
using neeksdk.Scripts.Game.Board;

namespace neeksdk.Scripts.Infrastructure.Services
{
    public class TileAnimationService
    {
        private readonly Queue<List<BoardTileData>> _animationQueue = new Queue<List<BoardTileData>>();

        public TileAnimationService()
        {
            
        }

        public void AddAnimationsToQueue(List<BoardTileData> animatedTiles) =>
            _animationQueue.Enqueue(animatedTiles);

        public bool HasAnimations() =>
            _animationQueue.Count > 0;

        public void PlayCollectTileAnimations()
        {
            foreach (List<BoardTileData> boardTileData in _animationQueue)
            {
                
            }
        }
    }
}