using UnityEngine;

namespace Assets.Core.Grid
{
    public static class GridUtils
    {
        public static int GetIndex(int x, int y, int rows)
        {
            return (x * rows) + y;
        }

        public static (int x, int y) GetCoordinates(int index, int rows)
        {
            var x = index / rows;
            var y = index % rows;
            return (x, y);
        }

        public static bool HasOutOfBoundsNeighbour(int x, int y, int rows, int cols)
        {
            return y - 1 < 0 || y + 1 >= rows || x - 1 < 0 || x + 1 >= cols;
        }

        public static Vector3 GetPositionByIndex(int index, int rows, int cellSize)
        {
            var (x, y) = GetCoordinates(index, rows);
            return GetPositionByCoordinates(x, y, cellSize);
        }

        public static Vector3 GetPositionByCoordinates(int x, int y, int cellSize)
        {
            return new Vector3(x * cellSize, 0f, y * cellSize);
        }
    }
}
