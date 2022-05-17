using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game.Board.BoardBackgrounds
{
    public class StandardBackground
    {
        private readonly BackgroundTypes _backgroundType;

        public ITile Tile { get; set; }

        public StandardBackground(BackgroundTypes backgroundType) =>
            _backgroundType = backgroundType;

        public bool CanPlaceTile() =>
            _backgroundType == BackgroundTypes.Standard;
    }
}