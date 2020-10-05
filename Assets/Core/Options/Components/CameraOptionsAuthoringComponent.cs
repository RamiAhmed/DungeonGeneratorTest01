using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace Assets.Core.Options.Components
{
    public sealed class CameraOptionsAuthoringComponent : BaseOptionsAuthoringComponent<CameraOptions>, IDeclareReferencedPrefabs
    {
        public GameObject cameraPrefab;

        public float cameraSmoothSpeed;
        public float cameraOffset;

        protected override CameraOptions GetOptions(GameObjectConversionSystem conversionSystem)
        {
            return new CameraOptions
            {
                cameraOffset = cameraOffset,
                cameraSmoothSpeed = cameraSmoothSpeed,
                cameraPrefab = conversionSystem.GetPrimaryEntity(cameraPrefab),
            };
        }

        void IDeclareReferencedPrefabs.DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
        {
            referencedPrefabs.Add(cameraPrefab);
        }
    }
}
