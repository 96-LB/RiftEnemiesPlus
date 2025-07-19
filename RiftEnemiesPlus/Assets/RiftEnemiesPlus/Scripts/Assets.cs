using System.IO;
using RhythmRift;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class Assets : MonoBehaviour {
    public Sprite[] redShieldSkeletonSprites;
    public Sprite[] blueShieldSkeletonSprites;
    public RRCustomEnemyDefinition[] enemies;

    internal static AssetsCopy Instance { get; private set; }
    internal static AssetBundle Bundle { get; private set; }

    public static void Initialize() {
        if (Instance) {
            Plugin.Log.LogWarning("Tried to initialize assets, but assets were already initialized!");
            return;
        }

        var bundlePath = Path.Combine(DIR.DIR.Dir, $"{Plugin.NAME}.bundle");
        Bundle = AssetBundle.LoadFromFile(bundlePath);

        Instance = Bundle.LoadAsset<AssetsCopy>("Assets.asset");
        if (!Instance) {
            Plugin.Log.LogFatal("Failed to initialize assets!");
            return;
        }


        var catalogPath = Path.Combine(DIR.DIR.Dir, $"catalog_{Plugin.NAME.ToLower()}.json");
        Addressables.LoadContentCatalogAsync(catalogPath).Completed += handle => {
            if (handle.Status == AsyncOperationStatus.Succeeded) {
                Plugin.Log.LogInfo("Successfully loaded content catalog for assets.");
            } else {
                Plugin.Log.LogFatal($"Failed to load content catalog: {handle.OperationException}");
            }
        };
    }

    public static void AddEnemiesToDatabase(RREnemyDatabase database) {
        if (!Instance) {
            Plugin.Log.LogError("Assets not initialized, cannot add enemies to database!");
            return;
        }
        if (Instance.enemies == null) {
            Plugin.Log.LogError("No enemies defined in assets, cannot add to database!");
            return;
        }
        if (database._enemyDefinitions == null) {
            Plugin.Log.LogError("Enemy database's enemy definitions list is null, cannot add enemies!");
            return;
        }
        foreach (var enemy in Instance.enemies) {
            if (enemy != null) {
                database._enemyDefinitions.Add(enemy.EnemyDefinition);
            }
        }
    }
}
