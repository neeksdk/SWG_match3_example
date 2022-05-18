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
            new Vector3(boardTileData.Row, boardTileData.Col, 0);

        public static TileType TileType(this BoardTileData boardTileData) =>
            boardTileData.Tile.TileType;
    }
}