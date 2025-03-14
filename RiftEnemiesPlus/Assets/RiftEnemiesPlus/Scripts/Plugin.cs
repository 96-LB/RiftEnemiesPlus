using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
    public const string GUID = "com.lalabuff.necrodancer.riftenemiesplus";
    public const string NAME = "RiftEnemiesPlus";
    public const string VERSION = "0.0.1";
     
    internal static ManualLogSource Log;
    
    internal void Awake() {
        Log = Logger;

        Harmony harmony = new(GUID);
        harmony.PatchAll();
                
        global::Config.Initialize(Config);
        Assets.Initialize();
        
        Log.LogInfo($"{NAME} v{VERSION} ({GUID}) has been loaded! Have fun!");
        foreach(var x in harmony.GetPatchedMethods()) {
            Log.LogInfo($"Patched {x}.");
        }
    }
}
