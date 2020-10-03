using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options
{
    [Serializable]
    public struct PlayerOptions : ISystemStateSharedComponentData, IEquatable<PlayerOptions>
    {
        public GameObject playerPrefab;
        public float playerMoveSpeed;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public override int GetHashCode()
        {
            return playerPrefab.GetHashCode() + playerMoveSpeed.GetHashCode();
        }

        bool IEquatable<PlayerOptions>.Equals(PlayerOptions other)
        {
            return playerPrefab == other.playerPrefab &&
                   playerMoveSpeed == other.playerMoveSpeed;
        }
    }
}
