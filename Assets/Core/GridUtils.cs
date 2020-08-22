namespace Assets.Core
{
    public static class GridUtils
    {
        public static int GetIndex(int x, int y, int rows)
        {
            return (x * rows) + y;
        }

        public static (int x, int y) GetCoordinates(int index, int rows, int cols, int cellSize)
        {
            var x = (index * cellSize) % cols;
            var y = (index * cellSize) / rows;
            return (x, y);
        }
    }
}
