using Unity.Entities;
using Unity.Mathematics;

namespace Assets.Core.Player
{
    public struct PlayerMovementComponentData : IComponentData
    {
        public float3 Velocity;
    }
}
