using Assets.Core.Options;
using System;
using System.Diagnostics;
using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Physics;
using Unity.Transforms;

namespace Assets.Core.Grid
{
    public class GridEnvironmentPlacerService : IDisposable
    {
        private readonly GridEnvironmentOptions _environmentOptions;
        private readonly EntityCommandBuffer _commandBuffer;
        private readonly GridGeneratorOptions _options;
        private readonly NativeArray<byte> _gridState;
        private readonly EntityManager _entityManager;

        private Entity[] _prefabs;
        private PhysicsCollider?[] _prefabColliders;

        public GridEnvironmentPlacerService(EntityManager entityManager, GridGeneratorOptions options, GridEnvironmentOptions environmentOptions, NativeArray<byte> gridState, EntityCommandBuffer commandBuffer)
        {
            _options = options;
            _gridState = gridState;
            _entityManager = entityManager;
            _commandBuffer = commandBuffer;
            _environmentOptions = environmentOptions;
        }

        public void Execute()
        {
            var watch = Stopwatch.StartNew();

            PreparePrefabs();

            InstantiatePrefabs();

            UnityEngine.Debug.Log($"{this} took {watch.Elapsed}");
        }

        private void InstantiatePrefabs()
        {
            for (int i = 0; i < _gridState.Length; i++)
            {
                var prefab = GetPrefab(i);
                var position = GridUtils.GetPositionByIndex(i, _options.gridRows, _options.cellSize);

                var entity = _commandBuffer.Instantiate(prefab);
                _commandBuffer.AddComponent(entity, new GridEntityComponentData
                {
                    Index = i,
                });

                _commandBuffer.SetComponent(entity, new Translation { Value = position });

                var collider = GetPhysicsCollider(i);
                if (collider == null || !collider.Value.IsValid)
                    continue;

                _commandBuffer.SetComponent(entity, new PhysicsCollider { Value = collider.Value.Value });
            }
        }

        private void PreparePrefabs()
        {
            _prefabs = new[]
            {
                _environmentOptions.exitPrefab,
                _environmentOptions.blockPrefab,
                _environmentOptions.pathPrefab
            };

            _prefabColliders = _prefabs
                .Select(prefab => 
                    _entityManager.HasComponent<PhysicsCollider>(prefab) 
                    ? _entityManager.GetComponentData<PhysicsCollider>(prefab) 
                    : (PhysicsCollider?) null)
                .ToArray();
        }

        private PhysicsCollider? GetPhysicsCollider(int index)
        {
            if (_gridState[index] == GridStateConstants.EXIT)
                return _prefabColliders[0];

            if (_gridState[index] == GridStateConstants.BLOCKED)
                return _prefabColliders[1];

            return _prefabColliders[2];
        }

        private Entity GetPrefab(int index)
        {
            if (_gridState[index] == GridStateConstants.EXIT)
                return _prefabs[0];

            if (_gridState[index] == GridStateConstants.BLOCKED)
                return _prefabs[1];

            return _prefabs[2];
        }

        public void Dispose()
        {
            _prefabs = null;
            _prefabColliders = null;
        }
    }
}
