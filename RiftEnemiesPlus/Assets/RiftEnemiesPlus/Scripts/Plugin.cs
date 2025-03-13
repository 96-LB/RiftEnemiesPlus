using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

[BepInPlugin(GUID, NAME, VERSION)]
public class Plugin : BaseUnityPlugin {
    const string GUID = "com.lalabuff.necrodancer.riftenemiesplus";
    const string NAME = "RiftEnemiesPlus";
    const string VERSION = "0.0.1";
     
    internal static ManualLogSource Log;
    
    internal static AssetBundle Assets { get; private set; }
    
    internal void Awake() {
        Log = Logger;

        Harmony harmony = new(GUID);
        harmony.PatchAll();
        
        Log.LogInfo($"{NAME} v{VERSION} ({GUID}) has been loaded! Have fun!");
        foreach(var x in harmony.GetPatchedMethods()) {
            Log.LogInfo($"Patched {x}.");
        }
        
        var assembly = Assembly.GetExecutingAssembly();
        var dir = Path.GetDirectoryName(assembly.Location);
        var path = Path.Combine(dir, "assetbundle.bundle");
        Assets = AssetBundle.LoadFromFile(path);
    }
}
