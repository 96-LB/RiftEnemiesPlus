using System.Reflection;
using HarmonyLib;
using RhythmRift.Enemies;
using UnityEngine;

using P = RhythmRift.Enemies.RRSkeletonEnemy;

[HarmonyPatch(typeof(P), nameof(P.OnSpawn))]
internal static class ShadowPatch_UpdateAnimations {
    private static void Postfix(
        Animation ____animationComponent,
        ref SpriteAnimationData ____shieldedMoveAnimData
    ) {
        var obj = Plugin.Assets.LoadAsset<GameObject>("ADC");
        var container = obj.GetComponent<AnimationDataContainer>();
        var data = container.animationData;
        data.AnimClip.legacy = true;
        ____animationComponent.AddClip(data.AnimClip, data.AnimClip.name);
        
        
        var animClip = ____shieldedMoveAnimData.Field<AnimationClip>("_animClip");
        var beatProgressToStartOn = ____shieldedMoveAnimData.Field<float>("_beatProgressToStartOn");
        var durationInBeats = ____shieldedMoveAnimData.Field<float>("_durationInBeats");
        var shouldIgnoreEnemyTempo = ____shieldedMoveAnimData.Field<bool>("_shouldIgnoreEnemyTempo");
        var animationSprites = ____shieldedMoveAnimData.Field<Sprite[]>("_animationSprites");
        
        Plugin.Log.LogError("******************");
        Plugin.Log.LogMessage($"animClip: {animClip}");
        Plugin.Log.LogMessage($"wrapMode: {animClip.Value.wrapMode}");
        Plugin.Log.LogMessage($"beatProgressToStartOn: {beatProgressToStartOn}");
        Plugin.Log.LogMessage($"durationInBeats: {durationInBeats}");
        Plugin.Log.LogMessage($"shouldIgnoreEnemyTempo: {shouldIgnoreEnemyTempo}");
        foreach(var x in animationSprites.Value) {
            Plugin.Log.LogMessage($"animationSprites: {x}");
        }
        foreach(var x in animClip.Value.events) {
            Plugin.Log.LogWarning("time: " + x.time);
            Plugin.Log.LogWarning("functionName: " + x.functionName);
            Plugin.Log.LogWarning("messageOptions: " + x.messageOptions);
            Plugin.Log.LogWarning("stringParameter: " + x.stringParameter);
            Plugin.Log.LogWarning("intParameter: " + x.intParameter);
            Plugin.Log.LogWarning("floatParameter: " + x.floatParameter);
            Plugin.Log.LogWarning("objectReferenceParameter: " + x.objectReferenceParameter);
        }
        animClip.Value.wrapMode = WrapMode.Loop;
        ____shieldedMoveAnimData = data;
    }
}


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


internal static class FieldRefExtensions {
    internal static FieldRef<V> Field<V>(this object instance, string name) {
        return new(instance, name);
    }
}
