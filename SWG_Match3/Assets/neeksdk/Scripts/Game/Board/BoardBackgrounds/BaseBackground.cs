using UnityEngine;

namespace neeksdk.Scripts.Game.Board.BoardBackgrounds
{
    public abstract class BaseBackground : IBackground
    {
        private readonly BackgroundMonoContainer _backgroundMonoContainer;
        
        public BackgroundType BackgroundType { get; }
        public IBackground Tile { get; set; }

        protected BaseBackground(BackgroundType backgroundType, BackgroundMonoContainer backgroundMonoContainer)
        {
            BackgroundType = backgroundType;
            _backgroundMonoContainer = backgroundMonoContainer;
        }

        public GameObject GameObject => _backgroundMonoContainer.gameObject;
        public BackgroundMonoContainer BackgroundMonoContainer => _backgroundMonoContainer;

        public bool CanPlaceTile() =>
            BackgroundType != BackgroundType.Empty;
    }
}