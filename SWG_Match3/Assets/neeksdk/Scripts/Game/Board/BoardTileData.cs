using neeksdk.Scripts.Game.Board.BoardBackgrounds;
using neeksdk.Scripts.Game.Board.BoardTiles;

namespace neeksdk.Scripts.Game.Board
{
    public class BoardTileData
    {
        public int Row;
        public int Col;
        public BackgroundType BackgroundType;
        public IBackground Background;
        public ITile Tile;
    }
}