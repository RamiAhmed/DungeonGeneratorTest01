using System;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options
{
    public struct CameraOptions : IComponentData, IEquatable<CameraOptions>
    {
        public Entity cameraPrefab;

        public float cameraSmoothSpeed;
        public float cameraOffset;

        public override string ToString()
        {
            return JsonUtility.ToJson(this);
        }

        public override int GetHashCode()
        {
            return cameraPrefab.GetHashCode() + cameraSmoothSpeed.GetHashCode() + cameraOffset.GetHashCode();
        }

        bool IEquatable<CameraOptions>.Equals(CameraOptions other)
        {
            return cameraPrefab == other.cameraPrefab &&
                   cameraSmoothSpeed == other.cameraSmoothSpeed &&
                   cameraOffset == other.cameraOffset;
        }
    }
}
