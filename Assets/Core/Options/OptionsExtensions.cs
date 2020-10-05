using Unity.Entities;

namespace Assets.Core.Options
{
    public static class OptionsExtensions
    {
        public static T GetOptions<T>(this SystemBase system) where T : struct, IComponentData
        {
            return system.GetSingleton<T>();
        }
    }
}
