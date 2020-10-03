using Assets.Core.Options;
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
        private CameraOptions _options;

        protected override void OnStartRunning()
        {
            base.OnStartRunning();
            _options = this.GetOptions<CameraOptions>();
        }

        protected override void OnUpdate()
        {
            var offset = _options.cameraOffset;
            var cameraSmoothSpeed = _options.cameraSmoothSpeed;
            var deltaTime = Time.DeltaTime;

            // check if player exist
            var player = LocalPlayer;
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
