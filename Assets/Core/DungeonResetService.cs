//using UnityEngine;

//namespace Assets.Core
//{
//    public class DungeonResetService
//    {
//        public void DestroyDungeon(DungeonResetOptions options)
//        {
//            if (options.parent == null)
//                return;

//            var parent = options.parent;
//            var childCount = parent.childCount;
//            for (int i = childCount - 1; i >= 0; i--)
//            {
//                GameObject.Destroy(parent.GetChild(i).gameObject);
//            }
//        }
//    }

//    public struct DungeonResetOptions
//    {
//        public Transform parent;
//    }
//}
