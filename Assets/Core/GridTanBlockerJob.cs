using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace Assets.Core
{
    public struct GridTanBlockerJob : IJobParallelFor
    {
        private readonly int _rows;
        private readonly float _sensitivity;

        private NativeArray<byte> _gridState;

        public GridTanBlockerJob(NativeArray<byte> gridState, int rows, float sensitivity)
        {
            _rows = rows;
            _sensitivity = sensitivity;
            _gridState = gridState;
        }

        public void Execute(int index)
        {
            var (x, y) = GridUtils.GetCoordinates(index, _rows);
            var up = y + 1;
            var down = y - 1;
            var left = x - 1;
            var right = x + 1;

            if (Mathf.Tan(up * left + right - down) > _sensitivity)
                _gridState[index] = GridStateConstants.BLOCKED;
        }
    }
}
