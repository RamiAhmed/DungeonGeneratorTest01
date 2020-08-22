using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core.Grid
{
    public struct GridFloodGateJob : IJob
    {
        private readonly int _cols;
        private readonly int _rows;

        [ReadOnly]
        private readonly NativeArray<byte> _floodState;

        private NativeArray<byte> _gridState;

        public GridFloodGateJob(NativeArray<byte> gridState, NativeArray<byte> floodState, int cols, int rows)
        {
            _cols = cols;
            _rows = rows;
            _gridState = gridState;
            _floodState = floodState;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();
            FloodGate();
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void FloodGate()
        { 
            for (var index = _cols; index < _gridState.Length - _cols; index++)
            {
                if (_floodState[index] != GridStateConstants.FLOODED)
                    continue;

                var (x, y) = GridUtils.GetCoordinates(index, _rows);
                var topLeft = GridUtils.GetIndex(x - 1, y + 1, _rows);
                var topRight = GridUtils.GetIndex(x + 1, y - 1, _rows);
                var botLeft = GridUtils.GetIndex(x - 1, y - 1, _rows);
                var botRight = GridUtils.GetIndex(x + 1, y - 1, _rows);

                int targetIndex;
                if (_gridState[topLeft] == GridStateConstants.FREE || _gridState[botLeft] == GridStateConstants.FREE)
                {
                    targetIndex = x - 1; // left cell
                }
                else if (_gridState[topRight] == GridStateConstants.FREE || _gridState[botRight] == GridStateConstants.FREE)
                {
                    targetIndex = x + 1; // right cell
                }
                else
                {
                    continue;
                }

                var (targetX, targetY) = GridUtils.GetCoordinates(targetIndex, _rows);
                if (GridUtils.HasOutOfBoundsNeighbour(targetX, targetY, _rows, _cols))
                    continue;

                //UnityEngine.Debug.Log($"index: {index} has {freeNeighbours} free neighbours, delta: {freeNeighbourIdx - index}");
                _gridState[targetIndex] = GridStateConstants.FREE;
            }
        }
    }
}
