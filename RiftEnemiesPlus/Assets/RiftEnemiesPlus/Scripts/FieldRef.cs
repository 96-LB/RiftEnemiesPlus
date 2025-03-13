using System.Reflection;

internal class FieldRef<V> {
    private readonly object instance;
    private readonly FieldInfo field;
    
    public V Value => (V)field.GetValue(instance);
    
    public FieldRef(object instance, FieldInfo field) {
        this.instance = instance;
        this.field = field;
    }
    
    public FieldRef(object instance, string name) :
        this(instance, instance.GetType().GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)) { }

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
