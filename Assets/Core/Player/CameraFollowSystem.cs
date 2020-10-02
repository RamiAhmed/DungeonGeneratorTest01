using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(PlayerMovementSystem))]
    public class CameraFollowSystem : BasePlayerDependentSystem
    {
        private DungeonOptions _options;

        protected override void OnCreate()
        {
            base.OnCreate();

            _options = DebugTestStarter.GetOptions(); // TODO: get options in proper way
        }

        protected override void OnUpdate()
        {
            var deltaTime = Time.DeltaTime;
            var offset = _options.cameraOffset;
            var cameraSmoothSpeed = _options.cameraSmoothSpeed;

            var player = LocalPlayer;

            // check if player exist
            if (player == Entity.Null)
                return;

            var target = EntityManager.GetComponentData<Translation>(player);

            Entities
                .WithName("CameraFollowSystem_ForEach")
                .WithBurst()
                .ForEach((ref Translation camPosition, ref Rotation camRotation, in PlayerCameraComponentData camera) =>
                {
                    // Follow the Player
                    float3 desiredPosition = target.Value + offset;
                    float3 smoothedPosition = math.lerp(camPosition.Value, desiredPosition, cameraSmoothSpeed * deltaTime);
                    camPosition.Value = smoothedPosition;

                    // Rotate Camera to the Player
                    float3 lookVector = target.Value - camPosition.Value;
                    Quaternion rotation = Quaternion.LookRotation(lookVector);
                    camRotation.Value = rotation;
                })
                .Schedule();
        }
    }
}
