using System.Linq;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;

namespace Assets.Core.Player
{
    public abstract class BasePlayerDependentSystem : SystemBase
    {
        private Entity _localPlayer;

        protected Entity LocalPlayer => _localPlayer == Entity.Null ? (_localPlayer = GetPlayerEntity(1)) : _localPlayer;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");
        }

        protected Entity GetPlayerEntity(int index)
        {
            var query = GetEntityQuery(ComponentType.ReadOnly<PlayerComponentData>());
            using var entities = query.ToEntityArray(Allocator.TempJob);

            return entities.SingleOrDefault(entity => EntityManager.GetComponentData<PlayerComponentData>(entity).Index == index);
        }
    }
}
