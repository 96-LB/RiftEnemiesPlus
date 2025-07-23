using UnityEngine;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "Assets", menuName = "RiftEnemiesPlus/Assets")]
    public class CustomAssets : ScriptableObject {
        public Sprite[] redShieldSkeletonSprites;
        public Sprite[] blueShieldSkeletonSprites;
        public CustomEnemyDefinition[] enemies;
    }
}
