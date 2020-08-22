using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public struct GridFloodFillJob : IJob
    {
        public const byte FREE = 0;
        public const byte UNSET = byte.MaxValue;
        public const byte FLOODED = byte.MaxValue - 1;

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
                _cellStack[i] = UNSET;

            for (int i = 0; i < _floodState.Length; i++)
                _floodState[i] = UNSET;

            // Add starting cell - must be free
            Push(_rows + 1); // we expect the cell at 1,1 to be free!

            do
            {
                var index = Pop();
                if (index == UNSET)
                    break;

                _floodState[index] = FLOODED;

                // Left
                var left = index - 1;
                if (_gridState[left] == FREE && _floodState[left] != FLOODED)
                    Push(left);

                // Right
                var right = index + 1;
                if (_gridState[right] == FREE && _floodState[right] != FLOODED)
                    Push(right);

                var up = index + _cols;
                if (_gridState[up] == FREE && _floodState[up] != FLOODED)
                    Push(up);

                var down = index - _cols;
                if (_gridState[down] == FREE && _floodState[down] != FLOODED)
                    Push(down);

            } while (_stackCount > 0);
        }

        // Stack push implementation - insert at first empty index
        private void Push(int index)
        {
            for (var i = 0; i < _cellStack.Length; i++)
            {
                if (_cellStack[i] != UNSET)
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
                if (_cellStack[i] == UNSET)
                {
                    continue;
                }

                var result = _cellStack[i];
                _cellStack[i] = UNSET;
                _stackCount -= 1;

                return result;
            }

            return UNSET;
        }
    }
}
