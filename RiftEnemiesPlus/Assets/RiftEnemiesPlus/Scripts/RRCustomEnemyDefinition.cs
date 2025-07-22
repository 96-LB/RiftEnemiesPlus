using RhythmRift;
using UnityEngine;
using UnityEngine.AddressableAssets;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "RRCustomEnemyDefinition", menuName = "RiftEnemiesPlus/RRCustomEnemyDefinition")]
    public class RRCustomEnemyDefinition : ScriptableObject {
        public int id;
        public string displayName;
        public HitData[] hits;
        public AssetReference prefabAssetReference;
        
        public int MaxHealth => hits?.Length ?? 0;

        public RREnemyDefinition GetDefinition(RREnemyDatabase database) => new(id, database) {
            _displayName = name,
            _maxHealth = MaxHealth,
            _totalHitsAddedToStage = MaxHealth,
            _prefabAssetReference = prefabAssetReference
        };
    }
}
