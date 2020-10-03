using Assets.Core.Options;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(PlayerInputSystem))]
    public class PlayerMovementSystem : SystemBase
    {
        private PlayerOptions _options;

        protected override void OnCreate()
        {
            UnityEngine.Debug.Log($"{this}: OnCreate");
        }

        protected override void OnStartRunning()
        {
            UnityEngine.Debug.Log($"{this}: OnStartRunning");
            _options = this.GetOptions<PlayerOptions>();
        }

        protected override void OnUpdate()
        {
            var playerMoveSpeed = _options.playerMoveSpeed;
            var deltaTime = Time.DeltaTime;

            Entities
                .WithName("PlayerMovementSystem_ForEach")
                .ForEach((ref PhysicsVelocity physicsVelocity, ref Rotation rotation, in PlayerMovementComponentData playerMove) =>
                {
                    physicsVelocity.Linear = math.mul(rotation.Value, playerMove.Velocity) * playerMoveSpeed * deltaTime;
                })
                .ScheduleParallel();
        }
    }
}
