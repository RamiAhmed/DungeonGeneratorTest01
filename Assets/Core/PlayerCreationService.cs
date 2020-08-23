using Assets.Core.Grid;
using UnityEngine;
using UnityStandardAssets.Cameras;

namespace Assets.Core
{
    public class PlayerCreationService
    {
        public GameObject CreatePlayer(PlayerCreationOptions options)
        {
            // Instantiate and place the player prefab
            var (startX, startY) = GridUtils.GetCoordinates(options.startIndex, options.gridRows);
            var startPos = GridUtils.GetPositionByIndex(startX, startY, options.cellSize) +
                new Vector3(options.cellSize * 0.5f, 1f, options.cellSize * 0.5f);
            var playerGo = GameObject.Instantiate(options.playerPrefab, startPos, Quaternion.identity);

            /// DEBUG STUFF
            GameObject.FindObjectOfType<FreeLookCam>().Target = playerGo.transform;

            return playerGo;
        }
    }

    public class PlayerCreationOptions
    {
        public GameObject playerPrefab;
        public int startIndex;
        public int gridRows;
        public int cellSize;
    }
}
