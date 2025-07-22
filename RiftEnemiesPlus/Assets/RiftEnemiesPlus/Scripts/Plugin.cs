using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;


namespace RiftEnemiesPlus {
    [BepInPlugin(GUID, NAME, VERSION)]
    public class Plugin : BaseUnityPlugin {
        public const string GUID = "com.lalabuff.necrodancer.riftenemiesplus";
        public const string NAME = "RiftEnemiesPlus";
        public const string VERSION = "0.0.1";
        public static string AssemblyDir => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        internal static ManualLogSource Log;
        
        internal void Awake() {
            Log = Logger;
            
            Harmony harmony = new(GUID);
            harmony.PatchAll();
            
            RiftEnemiesPlus.Config.Initialize(Config);
            Assets.Initialize();
            
            Log.LogInfo($"{NAME} v{VERSION} ({GUID}) has been loaded! Have fun!");
            foreach(var x in harmony.GetPatchedMethods()) {
                Log.LogInfo($"Patched {x}.");
            }
        }
    }
}
