using System.Diagnostics;
using System.Numerics;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Core.Grid
{
    public class GridEnvironmentPlacerService
    {
        private NativeArray<byte> _gridState;
        private EntityCommandBuffer _commandBuffer;
        private EntityManager _entityManager;
        private readonly GridPlacerOptions _options;

        private Entity[] _prefabs;

        public GridEnvironmentPlacerService(GridPlacerOptions options, NativeArray<byte> gridState, EntityCommandBuffer commandBuffer)
        {
            _options = options;
            _gridState = gridState;
            _commandBuffer = commandBuffer;
        }

        public GridEnvironmentPlacerService(GridPlacerOptions gridPlacerOptions, NativeArray<byte> gridState, EntityManager entityManager)
        {
            _options = gridPlacerOptions;
            _gridState = gridState;
            _entityManager = entityManager;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();

            PreparePrefabs();

            if (_entityManager != default)
            {
                ExecuteWithEntityManager();
            }
            else
            {
                ExecuteWithEntityCommandBuffer();
            }

            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void ExecuteWithEntityManager()
        {
            for (int i = 0; i < _gridState.Length; i++)
            {
                var prefab = GetPrefab(i);
                var position = GridUtils.GetPositionByIndex(i, _options.rows, _options.cellSize);

                var entity = _entityManager.Instantiate(prefab);
                _entityManager.AddComponentData(entity, new GridEntityComponentData
                {
                    Index = i,
                });

                _entityManager.SetComponentData(entity, new Translation { Value = position });
            }

            // Destroy the prefabs - clean up
            foreach (var prefab in _prefabs)
            {
                _entityManager.DestroyEntity(prefab);
            }
        }

        private void ExecuteWithEntityCommandBuffer()
        {
            for (int i = 0; i < _gridState.Length; i++)
            {
                var prefab = GetPrefab(i);
                var position = GridUtils.GetPositionByIndex(i, _options.rows, _options.cellSize);

                var entity = _commandBuffer.Instantiate(prefab);
                _commandBuffer.AddComponent(entity, new GridEntityComponentData
                {
                    Index = i,
                });

                _commandBuffer.SetComponent(entity, new Translation { Value = position });
            }

            // Destroy the prefabs - clean up
            foreach (var prefab in _prefabs)
            {
                _commandBuffer.DestroyEntity(prefab);
            }
        }

        private void PreparePrefabs()
        {
            var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);

            _prefabs = new[]
            {
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.exitPrefab, settings),
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.blockPrefab, settings),
                GameObjectConversionUtility.ConvertGameObjectHierarchy(_options.pathPrefab, settings)
            };
        }

        private Entity GetPrefab(int index)
        {
            if (_gridState[index] == GridStateConstants.EXIT)
                return _prefabs[0];

            if (_gridState[index] == GridStateConstants.BLOCKED)
                return _prefabs[1];

            return _prefabs[2];
        }
    }
}
