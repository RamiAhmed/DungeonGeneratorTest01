using Assets.Core.Grid;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Core
{
    [UpdateAfter(typeof(GridGeneratorSystem))]
    public class GridEnvironmentSystem : SystemBase
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

        private NativeArray<byte> GetGridState()
        {
            var gridEntityQuery = GetEntityQuery(ComponentType.ReadOnly<GridSharedSystemComponentData>());
            var entities = gridEntityQuery.ToEntityArray(Allocator.TempJob);

            var gridData = EntityManager.GetSharedComponentData<GridSharedSystemComponentData>(entities.Single());

            entities.Dispose();
            return gridData.GridState;
        }

        private void CreateGridCellEntities(NativeArray<byte> gridState)
        {
            var commandBuffer = _entityCommandBufferSystem.CreateCommandBuffer();
            _gridPlacerService = new GridEnvironmentPlacerService(
                DebugTestStarter.GetOptions().GetGridPlacerOptions(), // TODO: Get options in proper way
                gridState,
                commandBuffer);
            _gridPlacerService.Execute();

            _entityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }

        protected override void OnUpdate()
        {
            //UnityEngine.Debug.Log($"{this}: OnUpdate");

        }

        protected override void OnDestroy()
        {
            _gridPlacerService?.Dispose();
        }
    }
}
