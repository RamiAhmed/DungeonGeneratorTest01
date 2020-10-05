using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Options.Components
{
    public abstract class BaseOptionsAuthoringComponent<T> : MonoBehaviour, IConvertGameObjectToEntity where T : struct, IComponentData
    {
        void IConvertGameObjectToEntity.Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            var options = GetOptions(conversionSystem);
            dstManager.AddComponentData(entity, options);

            if (dstManager.HasComponent<Translation>(entity))
                dstManager.RemoveComponent<Translation>(entity);

            if (dstManager.HasComponent<Rotation>(entity))
                dstManager.RemoveComponent<Rotation>(entity);

            if (dstManager.HasComponent<LocalToWorld>(entity))
                dstManager.RemoveComponent<LocalToWorld>(entity);

            dstManager.CreateEntityQuery(typeof(T)).SetSingleton(options);
        }

        protected abstract T GetOptions(GameObjectConversionSystem conversionSystem);
    }
}
