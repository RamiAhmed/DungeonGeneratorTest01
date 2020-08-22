using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core.Grid
{
    public struct GridBoundsBlockerJob : IJobParallelFor
    {
        private readonly int _cols;
        private readonly int _rows;

        private NativeArray<byte> _gridState;

        public GridBoundsBlockerJob(NativeArray<byte> gridState, int cols, int rows)
        {
            _cols = cols;
            _rows = rows;
            _gridState = gridState;
        }

        public void Execute(int index)
        {
            var (x, y) = GridUtils.GetCoordinates(index, _rows);
            if (GridUtils.HasOutOfBoundsNeighbour(x, y, _rows, _cols))
                _gridState[index] = GridStateConstants.BLOCKED;
        }
    }
}
