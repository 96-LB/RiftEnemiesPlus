using System.IO;
using System.Reflection;
using UnityEngine;


public class Assets : MonoBehaviour {
    public Sprite[] redShieldSkeletonSprites;
    public Sprite[] blueShieldSkeletonSprites;
    
    internal static Assets Instance { get; private set; }
    internal static AssetBundle Bundle { get; private set; }
    
    public static void Initialize() {
        if(Instance) {
            Plugin.Log.LogWarning("Tried to initialize assets, but assets were already initialized!");
            return;
        }
        
        var assembly = Assembly.GetExecutingAssembly();
        var dir = Path.GetDirectoryName(assembly.Location);
        var path = Path.Combine(dir, $"{Plugin.NAME}.bundle");
        Bundle = AssetBundle.LoadFromFile(path);
        
        var obj = Bundle.LoadAsset<GameObject>("Presets.prefab");
        Instance = obj == null ? null : obj.GetComponent<Assets>();
        if(!Instance) {
            Plugin.Log.LogFatal("Failed to initialize assets!");
        }
    }
}
