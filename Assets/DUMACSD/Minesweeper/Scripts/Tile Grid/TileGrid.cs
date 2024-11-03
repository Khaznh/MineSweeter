using System.Collections.Generic;
using UnityEngine;

namespace Ilumisoft.Minesweeper
{
    public class TileGrid : MonoBehaviour
    {
        [SerializeField]
        int width = 5;

        [SerializeField]
        int height = 5;

        [SerializeField]
        float cellSize = 1.0f;

        public int Width { get => this.width; }
        public int Height { get => this.height; }

        public Vector3 BottomLeftCorner => transform.position - new Vector3(Width - 1, Height - 1, 0) / 2 * CellSize;

        public float CellSize { get => this.cellSize; set => this.cellSize = value; }

        Tile[,] tiles;

        private void Awake()
        {
            tiles = new Tile[width, height];
        }

        public bool IsValidTilePosition(int x, int y)
        {
            return x >= 0 && x < Width && y >= 0 && y < Height;
        }

        public void SetTile(int x, int y, Tile tile)
        {
            tiles[x, y] = tile;
        }

        public bool TryGetTile(Vector2Int gridPos, out Tile tile)
        {
            return TryGetTile(gridPos.x, gridPos.y, out tile);
        }

        public bool TryGetTile(int x, int y, out Tile tile)
        {
            tile = null;

            if (!IsValidTilePosition(x, y))
            {
                return false;
            }

            if (tiles[x, y] != null)
            {
                tile = tiles[x, y];
                return true;
            }

            return false;
        }

        public bool TryGetGridPosition(Tile tile, out Vector2Int gridPosition)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (tile == tiles[x, y])
                    {
                        gridPosition = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            gridPosition = default;
            return false;
        }

        public Vector3 GetWorldPosition(int x, int y)
        {
            return BottomLeftCorner + new Vector3(x, y, 0) * CellSize;
        }

        private void OnDrawGizmos()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3 position = BottomLeftCorner + new Vector3(x, y, 0);
                    Gizmos.DrawWireCube(position, Vector3.one * CellSize);
                }
            }
        }

        // Thêm hai phương thức sau vào bên trong lớp TileGrid

        public List<Tile> GetNeighbors(Tile tile)
        {
            List<Tile> neighbors = new List<Tile>();
            if (TryGetGridPosition(tile, out Vector2Int pos))
            {
                // Duyệt qua 8 hướng xung quanh
                for (int y = -1; y <= 1; y++)
                {
                    for (int x = -1; x <= 1; x++)
                    {
                        if (x == 0 && y == 0) continue; // Bỏ qua chính ô đó
                        int neighborX = pos.x + x;
                        int neighborY = pos.y + y;

                        if (IsValidTilePosition(neighborX, neighborY) && TryGetTile(neighborX, neighborY, out Tile neighborTile))
                        {
                            neighbors.Add(neighborTile);
                        }
                    }
                }
            }
            return neighbors;
        }

        public int GetNumberOfSurroundingBombs(Tile tile)
        {
            int bombCount = 0;
            var neighbors = GetNeighbors(tile);

            foreach (var neighbor in neighbors)
            {
                if (neighbor.CompareTag(Bomb.Tag)) // Giả định rằng ô có bom có thẻ `Bomb`
                {
                    bombCount++;
                }
            }

            return bombCount;
        }
    }
}
