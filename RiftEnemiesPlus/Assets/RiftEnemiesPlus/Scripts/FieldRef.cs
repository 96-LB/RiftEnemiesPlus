using System.Reflection;

internal class FieldRef<V> {
    private readonly object instance;
    private readonly FieldInfo field;
    
    public V Value => (V)field.GetValue(instance);
    
    public FieldRef(object instance, FieldInfo field) {
        this.instance = instance;
        this.field = field ?? throw new System.ArgumentNullException(nameof(field));
    }
    
    public FieldRef(object instance, string name) {
        this.instance = instance;
        field = instance.GetType().GetField(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
            ?? throw new System.ArgumentException($"Field '{name}' not found in type '{instance.GetType()}'.");
    }
    
    public FieldRef<V> Set(V value) {
        field.SetValue(instance, value);
        return this;
    }

    public override string ToString() {
        return $"[{instance}].{field.Name} = [{Value}]";
    }

    public static implicit operator V(FieldRef<V> fieldRef) {
        return fieldRef.Value;
    }
}
