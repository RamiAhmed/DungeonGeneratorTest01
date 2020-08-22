using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public class DungeonGeneratorService : IDisposable
    {
        private NativeArray<int> _cellStack;
        private NativeArray<byte> _floodState;
        private NativeArray<byte> _gridState;

        private NativeArray<JobHandle> _jobHandles = new NativeArray<JobHandle>(5, Allocator.TempJob);
        private JobHandle _jobHandle;
        private Stopwatch _timer;

        public void Generate(DungeonGeneratorOptions options)
        {
            _timer = Stopwatch.StartNew();

            var count = options.gridRows * options.gridCols;
            _gridState = new NativeArray<byte>(count, Allocator.TempJob);
            _cellStack = new NativeArray<int>(count, Allocator.TempJob);
            _floodState = new NativeArray<byte>(count, Allocator.TempJob);

            var gridBlockerJob = new GridBlockerJob(_gridState, options.gridCols, options.gridRows, options.sensitivity);
            var gridBlockerJobHandle = gridBlockerJob.Schedule();

            var floodFillJob = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows);
            var floodFillJobHandle = floodFillJob.Schedule(gridBlockerJobHandle);

            var gridFloodGateJob = new GridFloodGateJob(_gridState, _floodState, options.gridCols);
            var gridFloodGateJobHandle = gridFloodGateJob.Schedule(floodFillJobHandle);

            var floodFillJob2 = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows);
            var floodFillJob2Handle = floodFillJob2.Schedule(gridFloodGateJobHandle);

            var gridFillerJob = new GridFillerJob(_gridState, _floodState);
            var gridFillerJobHandle = gridFillerJob.Schedule(_gridState.Length, 1, floodFillJob2Handle);

            _jobHandles[0] = gridBlockerJobHandle;
            _jobHandles[1] = floodFillJobHandle;
            _jobHandles[2] = gridFloodGateJobHandle;
            _jobHandles[3] = floodFillJob2Handle;
            _jobHandles[4] = gridFillerJobHandle;

            _jobHandle = JobHandle.CombineDependencies(_jobHandles);
        }

        public (NativeArray<byte> gridState, NativeArray<byte> floodState) Complete()
        {
            _jobHandle.Complete();
            UnityEngine.Debug.Log($"Grid generation took {_timer.Elapsed}");
            return (_gridState, _floodState);
        }

        void IDisposable.Dispose()
        {
            _jobHandles.Dispose();
            _gridState.Dispose();
            _floodState.Dispose();
            _cellStack.Dispose();
        }
    }

    public struct DungeonGeneratorOptions
    {
        public int gridRows;
        public int gridCols;
        public float sensitivity;
    }
}
