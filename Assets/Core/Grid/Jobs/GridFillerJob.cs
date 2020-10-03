using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core.Grid
{
    public struct GridFillerJob : IJobParallelFor
    {
        [ReadOnly]
        private readonly NativeArray<byte> _floodState;

        private NativeArray<byte> _gridState;

        public GridFillerJob(NativeArray<byte> gridState, NativeArray<byte> floodState)
        {
            _gridState = gridState;
            _floodState = floodState;
        }

        public void Execute(int index)
        {
            if (_floodState[index] == GridStateConstants.FLOODED || _gridState[index] != GridStateConstants.FREE || _gridState[index] == GridStateConstants.START)
                return;

            //UnityEngine.Debug.Log($"Blocking cell at index: {index}");
            _gridState[index] = GridStateConstants.BLOCKED;
        }
    }
}
