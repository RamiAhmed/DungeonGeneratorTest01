using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Core
{
    public struct GridBlockerJob : IJob
    {
        private readonly int _cols;
        private readonly int _rows;
        private readonly float _sensitivity;

        private NativeArray<byte> _gridState;

        public GridBlockerJob(NativeArray<byte> gridState, int cols, int rows, float sensitivity)
        {
            _cols = cols;
            _rows = rows;
            _sensitivity = sensitivity;
            _gridState = gridState;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();
            for (int x = 0; x < _cols; x++)
            {
                for (int y = 0; y < _rows; y++)
                {
                    var i = GridUtils.GetIndex(x, y, _rows);
                    _gridState[i] = IsCellBlocked(x, y) ? GridStateConstants.BLOCKED : GridStateConstants.FREE;
                }
            }
            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private bool IsCellBlocked(int x, int y)
        { 
            //var sb = new StringBuilder();
            //sb.Append($"({x}, {y})");

            var up = y + 1;
            var down = y - 1;
            var left = x - 1;
            var right = x + 1;

            //sb.Append($" (up: {up}, down: {down}, left: {left}, right: {right})");
            
            if (up >= _rows || down < 0 ||
                left < 0 || right >= _cols)
            {
                // out of bounds neighbour -> block this cell
                return true;
            }

            if (Mathf.Tan(up * left + right - down) > _sensitivity)
            {
                return false;
            }

            var upIdx = GridUtils.GetIndex(x, up, _rows);
            var downIdx = GridUtils.GetIndex(x, down, _rows);
            var leftIdx = GridUtils.GetIndex(left, y, _rows);
            var rightIdx = GridUtils.GetIndex(right, y, _rows);

            //sb.Append($" UP STATE: {(_gridState[upIdx] == BLOCKED ? "BLOCKED" : "FREE")},");
            //sb.Append($" DOWN STATE: {(_gridState[downIdx] == BLOCKED ? "BLOCKED" : "FREE")},");
            //sb.Append($" LEFT STATE: {(_gridState[leftIdx] == BLOCKED ? "BLOCKED" : "FREE")},");
            //sb.Append($" RIGHT STATE: {(_gridState[rightIdx] == BLOCKED ? "BLOCKED" : "FREE")}");
            //Debug.Log(sb.ToString());

            if ((_gridState[upIdx] == GridStateConstants.FREE && _gridState[downIdx] == GridStateConstants.FREE) &&
                (_gridState[rightIdx] == GridStateConstants.FREE || _gridState[leftIdx] == GridStateConstants.FREE))
            {
                return true;
            }

            return false;
        }
    }
}
