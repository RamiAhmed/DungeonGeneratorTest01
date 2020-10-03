using Unity.Entities;

namespace Assets.Core.Options
{
    public class OptionsLoadSystem : SystemBase
    {
        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");

            var debugOptions = DebugTestStarter.Instance; // TODO: get options in proper way

            var entity = EntityManager.CreateEntity(
                typeof(CameraOptions),
                typeof(GridEnvironmentOptions), 
                typeof(GridGeneratorOptions), 
                typeof(PlayerOptions));

            EntityManager.SetSharedComponentData(entity, debugOptions.cameraOptions);
            EntityManager.SetSharedComponentData(entity, debugOptions.gridEnvironmentOptions);
            EntityManager.SetSharedComponentData(entity, debugOptions.gridGeneratorOptions);
            EntityManager.SetSharedComponentData(entity, debugOptions.playerOptions);
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
