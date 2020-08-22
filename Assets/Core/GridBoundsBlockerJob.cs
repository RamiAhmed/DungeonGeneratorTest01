using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public struct GridBoundsBlockerJob : IJobParallelFor
    {
        private readonly int _cols;
        private readonly int _rows;
        private readonly float _sensitivity;

        private NativeArray<byte> _gridState;

        public GridBoundsBlockerJob(NativeArray<byte> gridState, int cols, int rows, float sensitivity)
        {
            _cols = cols;
            _rows = rows;
            _sensitivity = sensitivity;
            _gridState = gridState;
        }

        public void Execute(int index)
        {
            var watch = Stopwatch.StartNew();

            var (x, y) = GridUtils.GetCoordinates(index, _rows);
            if (GridUtils.HasOutOfBoundsNeighbour(x, y, _rows, _cols))
                _gridState[index] = GridStateConstants.BLOCKED;
            
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }
    }
}
