using Assets.Core.Grid;
using Assets.Core.Options;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

namespace Assets.Core
{
    public class GridGeneratorSystem : SystemBase
    { 
        private NativeArray<int> _cellStack;
        private NativeArray<byte> _floodState;
        private NativeArray<byte> _gridState;

        private NativeArray<JobHandle> _jobHandles = new NativeArray<JobHandle>(10, Allocator.TempJob);
        private JobHandle _jobHandle;
        private System.Diagnostics.Stopwatch _timer;
        private bool _generated;

        public void Generate()
        {
            if (_generated)
                return;

            _timer = System.Diagnostics.Stopwatch.StartNew();

            var options = this.GetOptions<GridGeneratorOptions>();
            var count = options.gridRows * options.gridCols;
            _gridState = new NativeArray<byte>(count, Allocator.Persistent);
            _cellStack = new NativeArray<int>(count, Allocator.TempJob);
            _floodState = new NativeArray<byte>(count, Allocator.TempJob);

            // Mark starting cell as such
            var startIndex = GridUtils.GetIndex(options.startX, options.startY, options.gridRows);
            _gridState[startIndex] = GridStateConstants.START;

            var gridBoundsBlockerJob = new GridBoundsBlockerJob(_gridState, options.gridCols, options.gridRows);
            var gridBoundsBlockerJobHandle = gridBoundsBlockerJob.Schedule(_gridState.Length, options.innerloopBatchCount);

            var gridTanBlockerJob = new GridTanBlockerJob(_gridState, options.gridRows, options.sensitivity);
            var gridTanBlockerJobHandle = gridTanBlockerJob.Schedule(_gridState.Length, options.innerloopBatchCount, gridBoundsBlockerJobHandle);

            var floodFillJob = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows, startIndex);
            var floodFillJobHandle = floodFillJob.Schedule(gridTanBlockerJobHandle);

            var gridFloodGateJob = new GridFloodGateJob(_gridState, _floodState, options.gridCols, options.gridRows);
            var gridFloodGateJobHandle = gridFloodGateJob.Schedule(floodFillJobHandle);

            var floodFillJob2 = new GridFloodFillJob(_gridState, _cellStack, _floodState, options.gridCols, options.gridRows, startIndex);
            var floodFillJob2Handle = floodFillJob2.Schedule(gridFloodGateJobHandle);

            var gridFillerJob = new GridFillerJob(_gridState, _floodState);
            var gridFillerJobHandle = gridFillerJob.Schedule(_gridState.Length, options.innerloopBatchCount, floodFillJob2Handle);

            var gridExitJob = new GridExitJob(_gridState, _floodState, options.gridCols, options.gridRows, startIndex);
            var gridExitJobHandle = gridExitJob.Schedule(gridFillerJobHandle);

            _jobHandles[0] = gridBoundsBlockerJobHandle;
            _jobHandles[1] = gridTanBlockerJobHandle;
            _jobHandles[2] = floodFillJobHandle;
            _jobHandles[3] = gridFloodGateJobHandle;
            _jobHandles[4] = floodFillJob2Handle;
            _jobHandles[5] = gridFillerJobHandle;
            _jobHandles[6] = gridExitJobHandle;

            _jobHandle = JobHandle.CombineDependencies(_jobHandles);
            UnityEngine.Debug.Log($"{this}: Grid jobs setup time: {_timer.Elapsed}");
        }

        private void CreateGridEntity()
        {
            var gridArchetype = EntityManager.CreateArchetype(typeof(GridSharedSystemComponentData));
            var gridEntity = EntityManager.CreateEntity(gridArchetype);
            EntityManager.SetSharedComponentData(gridEntity, new GridSharedSystemComponentData
            {
                GridState = _gridState
            });
        }

        private void Complete()
        {
            if (_generated)
                return;

            _jobHandle.Complete();
            UnityEngine.Debug.Log($"{this}: Grid generation completion time: {_timer.Elapsed}");

            CreateGridEntity();
            UnityEngine.Debug.Log($"{this}: Grid entity creation time: {_timer.Elapsed}");

            _jobHandles.Dispose();
            _floodState.Dispose();
            _cellStack.Dispose();

            _generated = true;
        }

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");
            Generate();
        }

        protected override void OnUpdate()
        {
            Complete();
        }

        protected override void OnDestroy()
        {
            _generated = false;
            _gridState.Dispose();
        }
    }
}
