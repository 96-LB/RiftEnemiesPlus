using UnityEngine;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "Assets", menuName = "RiftEnemiesPlus/AssetsObj")]
    public class AssetsObj : ScriptableObject {
        public Sprite[] redShieldSkeletonSprites;
        public Sprite[] blueShieldSkeletonSprites;
        public RRCustomEnemyDefinition[] enemies;
    }
}