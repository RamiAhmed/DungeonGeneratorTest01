using Assets.Core.Grid;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Core
{
    public abstract class BaseGridDependentSystem : SystemBase
    {
        protected NativeArray<byte> GetGridState()
        {
            var gridEntityQuery = GetEntityQuery(ComponentType.ReadOnly<GridSharedSystemComponentData>());
            using var entities = gridEntityQuery.ToEntityArray(Allocator.TempJob);

            var gridData = EntityManager.GetSharedComponentData<GridSharedSystemComponentData>(entities.Single());

            return gridData.GridState;
        }
    }
}
