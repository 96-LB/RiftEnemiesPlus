using RhythmRift;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "CustomEnemyDefinition", menuName = "RiftEnemiesPlus/EnemyDefinition")]
    public class CustomEnemyDefinition : ScriptableObject {
        public int id;
        public string displayName;
        public AssetReference prefabAssetReference;
        public HitData[] hits;
        public CustomAnimation deathAnimation;

        public int MaxHealth => hits?.Length ?? 0;

        public RREnemyDefinition GetDefinition(RREnemyDatabase database) => new(id, database) {
            _displayName = name,
            _maxHealth = MaxHealth,
            _totalHitsAddedToStage = MaxHealth,
            _prefabAssetReference = prefabAssetReference
        };

        public HitData GetHit(int num) {
            if(hits == null || num < 0 || num >= hits.Length || hits[num] == null) {
                return CreateInstance<HitData>();
            }
            return hits[num];
        }
    }
}
