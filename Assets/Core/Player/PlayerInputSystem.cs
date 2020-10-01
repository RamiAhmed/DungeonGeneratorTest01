using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Player
{
    [UpdateAfter(typeof(PlayerCreationSystem))]
    public class PlayerInputSystem : BasePlayerDependentSystem
    {
        protected override void OnUpdate()
        {
            var velocity = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
            {
                velocity.z += 1f;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                velocity.z -= 1f;
            }

            if (Input.GetKey(KeyCode.A))
            {
                velocity.x -= 1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                velocity.x += 1f;
            }

            EntityManager.SetComponentData(LocalPlayer, new PlayerMovementComponentData { Velocity = velocity });
        }
    }
}
