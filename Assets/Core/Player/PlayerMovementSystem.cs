using Unity.Entities;
using Unity.Transforms;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(PlayerInputSystem))]
    public class PlayerMovementSystem : SystemBase
    {
        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            Entities
                .WithName("PlayerMovementSystem_ForEach")
                .ForEach((ref Translation translation, in PlayerMovementComponentData playerMove) =>
                {
                    translation.Value += playerMove.Velocity * deltaTime;
                })
                .ScheduleParallel();
        }
    }
}
