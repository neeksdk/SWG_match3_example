using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game.Board.BoardBackgrounds
{
    public class StandardBackground
    {
        private readonly BackgroundType _backgroundType;

        public ITile Tile { get; set; }

        public StandardBackground(BackgroundType backgroundType) =>
            _backgroundType = backgroundType;

        public bool CanPlaceTile() =>
            _backgroundType != BackgroundType.Empty;
    }
}