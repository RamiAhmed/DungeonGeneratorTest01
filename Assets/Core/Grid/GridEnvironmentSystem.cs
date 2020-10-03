using Assets.Core.Grid;
using Assets.Core.Options;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Core
{
    [UpdateAfter(typeof(GridGeneratorSystem))]
    public class GridEnvironmentSystem : BaseGridDependentSystem
    {
        private BeginInitializationEntityCommandBufferSystem _entityCommandBufferSystem;
        private GridEnvironmentPlacerService _gridPlacerService;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
            _entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");

            var gridState = GetGridState();
            CreateGridCellEntities(gridState);
        }

        private void CreateGridCellEntities(NativeArray<byte> gridState)
        {
            var generatorOptions = this.GetOptions<GridGeneratorOptions>();
            var environmentOptions = this.GetOptions<GridEnvironmentOptions>();

            var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();
            _gridPlacerService = new GridEnvironmentPlacerService(
                EntityManager,
                generatorOptions,
                environmentOptions,
                gridState,
                commandBuffer);
            _gridPlacerService.Execute();

            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }

        protected override void OnDestroy()
        {
            _gridPlacerService?.Dispose();
        }
    }
}
