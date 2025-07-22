using UnityEngine;


namespace RiftEnemiesPlus {
    internal static class GameObjectExtensions {
        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
            if(gameObject.TryGetComponent<T>(out var component)) {
                return component;
            }
            return gameObject.AddComponent<T>();
        }
    }
}
