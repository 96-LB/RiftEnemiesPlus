using RhythmRift;
using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(fileName = "RRCustomEnemyDefinition", menuName = "RiftEnemiesPlus/RRCustomEnemyDefinition")]
public class RRCustomEnemyDefinition : ScriptableObject {
    public int id;
    public string displayName;
    public HitData[] hits;
    public AssetReference prefabAssetReference;
    
    public int MaxHealth => hits?.Length ?? 0;
    
    public RREnemyDefinition EnemyDefinition => new() {
        _id = id,
        _displayName = name,
        _maxHealth = MaxHealth,
        _totalHitsAddedToStage = MaxHealth,
        _totalEnemiesGenerated = 1,
        _playerDamage = 1,
        _prefabAssetReference = prefabAssetReference
    };
}
