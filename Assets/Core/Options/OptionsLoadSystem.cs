using Unity.Entities;

namespace Assets.Core.Options
{
    public class OptionsLoadSystem : SystemBase
    {
        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");

            var debugOptions = DebugTestStarter.Instance; // TODO: get options in proper way

            CreateOptionsEntity(debugOptions.cameraOptions);
            CreateOptionsEntity(debugOptions.gridEnvironmentOptions);
            CreateOptionsEntity(debugOptions.gridGeneratorOptions);
            CreateOptionsEntity(debugOptions.playerOptions);
        }

        private void CreateOptionsEntity<T>(T options) where T : struct, ISystemStateSharedComponentData
        {
            var entity = EntityManager.CreateEntity(typeof(T));
            EntityManager.SetSharedComponentData(entity, options);
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");
        }

        protected override void OnUpdate()
        {
            /* NOOP */
        }
    }
}
