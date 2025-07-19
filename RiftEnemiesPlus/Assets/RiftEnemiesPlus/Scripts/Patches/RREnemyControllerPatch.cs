using System.Collections.Generic;
using HarmonyLib;
using RhythmRift;

[HarmonyPatch(typeof(RREnemyController))]
public static class RREnemyControllerPatch {
    [HarmonyPatch(nameof(RREnemyController.Initialize))]
    [HarmonyPrefix]
    public static void Initialize(RREnemyController __instance) {
        if (__instance._enemyDatabase != null) {
            Assets.AddEnemiesToDatabase(__instance._enemyDatabase);
            Plugin.Log.LogWarning(__instance._enemyDatabase._enemyDefinitions.Count);
        } else {
            Plugin.Log.LogError("RREnemyController's enemy database is null, cannot add enemies!");
        }
    }
    
    [HarmonyPatch(nameof(RREnemyController.Initialize))]
    [HarmonyPostfix]
    public static void Initialize_Post(Dictionary<int, int> enemiesToPreloadByCountAndId, RREnemyController __instance) {
        foreach(var x in enemiesToPreloadByCountAndId) {
            Plugin.Log.LogInfo($"Preloading {x.Value} enemies with ID {x.Key}");
        }
    }
}
