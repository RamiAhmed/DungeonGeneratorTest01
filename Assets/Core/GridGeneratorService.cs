﻿using Assets.Core.Grid;
using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Jobs;

namespace Assets.Core
{
    public class GridGeneratorService : IDisposable
    {
        private NativeArray<int> _cellStack;
        private NativeArray<byte> _floodState;
        private NativeArray<byte> _gridState;

        private NativeArray<JobHandle> _jobHandles = new NativeArray<JobHandle>(10, Allocator.TempJob);
        private JobHandle _jobHandle;
        private Stopwatch _timer;

        public void Generate(GridGeneratorOptions options)
        {
            _timer = Stopwatch.StartNew();

            var count = options.gridRows * options.gridCols;
            _gridState = new NativeArray<byte>(count, options.allocatorType);
            _cellStack = new NativeArray<int>(count, options.allocatorType);
            _floodState = new NativeArray<byte>(count, options.allocatorType);

            // Mark starting cell as such
            _gridState[options.startIndex] = GridStateConstants.START;

            var gridBoundsBlockerJob = new GridBoundsBlockerJob(_gridState, options.gridCols, options.gridRows);
            var gridBoundsBlockerJobHandle = gridBoundsBlockerJob.Schedule(_gridState.Length, options.innerloopBatchCount);

            var gridTanBlockerJob = new GridTanBlockerJob(_gridState, options.gridRows, options.sensitivity);
            var gridTanBlockerJobHandle = gridTanBlockerJob.Schedule(_gridState.Length, options.innerloopBatchCount, gridBoundsBlockerJobHandle);

            var floodFillJob = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows, options.startIndex);
            var floodFillJobHandle = floodFillJob.Schedule(gridTanBlockerJobHandle);

            var gridFloodGateJob = new GridFloodGateJob(_gridState, _floodState, options.gridCols, options.gridRows);
            var gridFloodGateJobHandle = gridFloodGateJob.Schedule(floodFillJobHandle);

            var floodFillJob2 = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows, options.startIndex);
            var floodFillJob2Handle = floodFillJob2.Schedule(gridFloodGateJobHandle);

            var gridFillerJob = new GridFillerJob(_gridState, _floodState);
            var gridFillerJobHandle = gridFillerJob.Schedule(_gridState.Length, options.innerloopBatchCount, floodFillJob2Handle);

            var gridExitJob = new GridExitJob(_gridState, _floodState, options.gridCols, options.gridRows, options.startIndex);
            var gridExitJobHandle = gridExitJob.Schedule(gridFillerJobHandle);

            _jobHandles[0] = gridBoundsBlockerJobHandle;
            _jobHandles[1] = gridTanBlockerJobHandle;
            _jobHandles[2] = floodFillJobHandle;
            _jobHandles[3] = gridFloodGateJobHandle;
            _jobHandles[4] = floodFillJob2Handle;
            _jobHandles[5] = gridFillerJobHandle;
            _jobHandles[6] = gridExitJobHandle;

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

    [Serializable]
    public class GridGeneratorOptions
    {
        public int gridRows;
        public int gridCols;
        public float sensitivity;
        public int innerloopBatchCount;
        public Allocator allocatorType;
        public int startIndex;

        public override string ToString()
        {
            return UnityEngine.JsonUtility.ToJson(this);
        }
    }
}
