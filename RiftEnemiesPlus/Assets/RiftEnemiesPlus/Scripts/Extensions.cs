using UnityEngine;

internal static class FieldRefExtensions {
    public static FieldRef<V> Field<V>(this object instance, string name) {
        return new(instance, name);
    }
}

internal static class GameObjectExtensions {
    public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component {
        if(gameObject.TryGetComponent<T>(out var component)) {
            return component;
        }
        return gameObject.AddComponent<T>();
    }
}