using RhythmRift.Enemies;
using UnityEngine;

public static class DebugUtil {
    private static void Log(object message) {
        Plugin.Log.LogMessage(message);
    }
    
    private static void Header(object message) {
        Log($"*****{message}*****");
    }
    
    private static void Footer(object message) {
        Log(new string('*', message.ToString().Length + 10));
    }
    
    public static void Dump(SpriteAnimationData data) {
        Header(data);
        Log(data.Field<float>("_beatProgressToStartOn"));
        Log(data.Field<float>("_durationInBeats"));
        Log(data.Field<bool>("_shouldIgnoreEnemyTempo"));
        Dump(data.Field<AnimationClip>("_animClip").Value);
        var sprites = data.Field<Sprite[]>("_animationSprites").Value;
        Log($"{sprites.Length} sprites:");
        foreach(var x in sprites) {
            Dump(x);
        }
        Footer(data);
    }
    
    public static void Dump(AnimationClip clip) {
        Header(clip);
        Log($"wrap mode {clip.wrapMode}");
        Log($"{clip.frameRate} fps");
        Log($"{clip.length} seconds");
        Log($"{clip.events.Length} events:");
        foreach(var x in clip.events) {
            Log($"{x.time}: {x.functionName}({x.stringParameter}, {x.floatParameter}, {x.intParameter}, {x.objectReferenceParameter}) {x.messageOptions}");
        }
        Footer(clip);
    }
    
    public static void Dump(Sprite sprite) {
        Header(sprite);
        Log($"rect {sprite.rect}");
        Log($"pivot {sprite.pivot}");
        Log($"border {sprite.border}");
        Log($"pixels per unit {sprite.pixelsPerUnit}");
        Log($"packing mode {sprite.packingMode}");
        Footer(sprite);
    }
}