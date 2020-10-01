using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core.Grid
{
    public struct GridExitJob : IJob
    {
        private readonly int _gridCols;
        private readonly int _gridRows;
        private readonly int _startIndex;

        [ReadOnly]
        private readonly NativeArray<byte> _floodState;

        [WriteOnly]
        private NativeArray<byte> _gridState;

        public GridExitJob(NativeArray<byte> gridState, NativeArray<byte> floodState, int gridCols, int gridRows, int startIndex)
        {
            _gridCols = gridCols;
            _gridRows = gridRows;
            _floodState = floodState;
            _gridState = gridState;
            _startIndex = startIndex;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();
            var (startX, startY) = GridUtils.GetCoordinates(_startIndex, _gridRows);
            int bestDelta = 0, bestIndex = -1;
            for (int x = 1; x < _gridCols - 1; x++)
            {
                for (int y = 1; y < _gridRows - 1; y++)
                {
                    var index = GridUtils.GetIndex(x, y, _gridRows);
                    if (_floodState[index] != GridStateConstants.FLOODED)
                        continue;

                    var delta = Math.Abs(x - startX) + Math.Abs(y - startY);
                    if (delta <= bestDelta)
                        continue;

                    bestDelta = delta;
                    bestIndex = index;
                }
            }

            if (bestIndex == -1)
                throw new IndexOutOfRangeException();

            _gridState[bestIndex] = GridStateConstants.EXIT;
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }
    }
}
