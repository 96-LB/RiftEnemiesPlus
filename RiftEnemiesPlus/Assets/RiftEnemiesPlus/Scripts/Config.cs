using BepInEx.Configuration;


public static class Config {
    public class ConfigGroup {
        private readonly ConfigFile config;
        private readonly string group;
        public ConfigGroup(ConfigFile config, string group) {
            this.config = config;
            this.group = group;
        }
        
        public ConfigEntry<T> Bind<T>(string key, T defaultValue, string description) {
            return config.Bind(group, key, defaultValue, description);
        }

        public ConfigEntry<T> Bind<T>(string key, T defaultValue, ConfigDescription description = null) {
            return config.Bind(group, key, defaultValue, description);
        }
    }

    public static class AssetSwaps {
        public static bool BlueShields => blueShields.Value;
        private static ConfigEntry<bool> blueShields;
        
        public static bool RedShields => redShields.Value;
        private static ConfigEntry<bool> redShields;

        public static void Initialize(ConfigGroup config) {
            blueShields = config.Bind("Blue Shields", true, "Convert double-shielded skeletons into blue-shielded skeletons.");
            redShields = config.Bind("Red Shields", true, "Convert shielded skeletons into red-shielded skeletons.");
        }
    }
    
    public static void Initialize(ConfigFile config) {
        AssetSwaps.Initialize(new(config, "Asset Swaps"));
    }
}
