using UnityEngine;

namespace Assets.Core
{
    public class PlayerResetService
    {
        public void ResetPlayer(GameObject player)
        {
            if (player?.gameObject == null)
            {
                return;
            }

            GameObject.Destroy(player.gameObject);
        }
    }
}
