using System;
using System.Linq;
using Unity.Collections;
using Unity.Entities;

namespace Assets.Core.Options
{
    public static class OptionsExtensions
    {
        public static T GetOptions<T>(this SystemBase system) where T : struct, ISystemStateSharedComponentData
        {
            using var entities = system.EntityManager.CreateEntityQuery(typeof(T))
                .ToEntityArray(Allocator.TempJob);
            if (!entities.Any())
                throw new ArgumentOutOfRangeException(nameof(T));

            return system.EntityManager.GetSharedComponentData<T>(entities.Single());
        }
    }
}
