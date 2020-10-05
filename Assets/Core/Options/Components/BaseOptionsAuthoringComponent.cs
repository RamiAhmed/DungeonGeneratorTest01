using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options.Components
{
    public abstract class BaseOptionsAuthoringComponent<T> : MonoBehaviour, IConvertGameObjectToEntity where T : struct, ISystemStateSharedComponentData
    {
        void IConvertGameObjectToEntity.Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
        {
            dstManager.AddSharedComponentData(entity, GetOptions(conversionSystem));
        }

        protected abstract T GetOptions(GameObjectConversionSystem conversionSystem);
    }
}
