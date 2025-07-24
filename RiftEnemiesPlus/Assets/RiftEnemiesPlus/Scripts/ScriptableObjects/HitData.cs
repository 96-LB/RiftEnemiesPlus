using UnityEngine;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "HitData", menuName = "RiftEnemiesPlus/HitData")]
    public class HitData : ScriptableObject {
        public float delay;
        public int offset;
        public CustomAnimation movementAnimation;
        public CustomAnimation attackAnimation;
        public float animationSpeed = 1f;
    }
}
