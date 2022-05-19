using neeksdk.Scripts.Game.Board;
using neeksdk.Scripts.Game.Board.BoardTiles;
using UnityEngine;

namespace neeksdk.Scripts.Extensions
{
    public static class BoardExtensions
    {
        public static bool IsInsideGridBounds(this BoardData boardData, int row, int col) =>
            row >= 0 && row < boardData.Rows && col >= 0 && col < boardData.Cols;
        
        public static Vector3 BoardToVectorCoords(this BoardTileData boardTileData) =>
            new Vector3(boardTileData.Coords.Row, boardTileData.Coords.Col, 0);

        public static TileType TileType(this BoardTileData boardTileData) =>
            boardTileData.Tile.TileType;

        public static bool IsNeighbourTile(this ITile tile, ITile comparedTile)
        {
            if (tile.Coords.Row == comparedTile.Coords.Row)
            {
                if (tile.Coords.Col == comparedTile.Coords.Col + 1 || tile.Coords.Col == comparedTile.Coords.Col - 1)
                {
                    return true;
                }
            }

            if (tile.Coords.Col == comparedTile.Coords.Col)
            {
                if (tile.Coords.Row == comparedTile.Coords.Row + 1 || tile.Coords.Row == comparedTile.Coords.Row - 1)
                {
                    return true;
                }
            }

            return false;
        }
    }
}