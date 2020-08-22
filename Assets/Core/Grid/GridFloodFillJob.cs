using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public struct GridFloodFillJob : IJob
    {
        private readonly int _cols;
        private readonly int _rows;

        [ReadOnly]
        private readonly NativeArray<byte> _gridState;

        private NativeArray<byte> _floodState;
        private NativeArray<int> _cellStack;

        private int _stackCount;

        public GridFloodFillJob(NativeArray<byte> gridState, NativeArray<int> cellStack, NativeArray<byte> floodState, int cols, int rows)
        {
            _cols = cols;
            _rows = rows;
            _gridState = gridState;
            _cellStack = cellStack;
            _floodState = floodState;
            
            _stackCount = 0;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();
            FloodFill();
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void FloodFill() 
        { 
            // we use byte.MaxValue as an 'unset' state value
            for (int i = 0; i < _cellStack.Length; i++)
                _cellStack[i] = GridStateConstants.UNSET;

            for (int i = 0; i < _floodState.Length; i++)
                _floodState[i] = GridStateConstants.UNSET;

            // Add starting cell - must be free
            Push(GridUtils.GetIndex(1, 1, _rows)); // we expect the cell at 1,1 to be free!

            do
            {
                var index = Pop();
                if (index == GridStateConstants.UNSET)
                    break;

                _floodState[index] = GridStateConstants.FLOODED;

                var (x, y) = GridUtils.GetCoordinates(index, _rows);
                if (GridUtils.HasOutOfBoundsNeighbour(x, y, _rows, _cols))
                    continue;

                // Left
                var left = GridUtils.GetIndex(x - 1, y, _rows);
                if (_gridState[left] == GridStateConstants.FREE && _floodState[left] != GridStateConstants.FLOODED)
                    Push(left);

                // Right
                var right = GridUtils.GetIndex(x + 1, y, _rows);
                if (_gridState[right] == GridStateConstants.FREE && _floodState[right] != GridStateConstants.FLOODED)
                    Push(right);

                var up = GridUtils.GetIndex(x, y + 1, _rows);
                if (_gridState[up] == GridStateConstants.FREE && _floodState[up] != GridStateConstants.FLOODED)
                    Push(up);

                var down = GridUtils.GetIndex(x, y - 1, _rows);
                if (_gridState[down] == GridStateConstants.FREE && _floodState[down] != GridStateConstants.FLOODED)
                    Push(down);

            } while (_stackCount > 0);
        }

        // Stack push implementation - insert at first empty index
        private void Push(int index)
        {
            for (var i = 0; i < _cellStack.Length; i++)
            {
                if (_cellStack[i] != GridStateConstants.UNSET)
                {
                    continue;
                }

                _cellStack[i] = index;
                _stackCount += 1;
                return;
            }

            throw new IndexOutOfRangeException();
        }

        // Stack pop implementation - take the first non-empty element
        private int Pop()
        {
            for (var i = 0; i < _cellStack.Length; i++)
            {
                if (_cellStack[i] == GridStateConstants.UNSET)
                {
                    continue;
                }

                var result = _cellStack[i];
                _cellStack[i] = GridStateConstants.UNSET;
                _stackCount -= 1;

                return result;
            }

            return GridStateConstants.UNSET;
        }
    }
}
