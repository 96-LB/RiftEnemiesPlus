using RhythmRift.Enemies;
using UnityEngine;

public static class DebugUtil {
    private static void PrintAllComponents(GameObject gameObject, int depth = 1) {
        foreach(var component in gameObject.GetComponents<Component>()) {
            Plugin.Log.LogWarning($"{new string(' ', depth * 2)}• {component.GetType().Name}");
        }
    }

    internal static void PrintAllComponents(GameObject gameObject) {
        Plugin.Log.LogWarning($"Components of [{gameObject}]:");
        PrintAllComponents(gameObject, 1);
    }
    internal static void PrintAllComponents(Transform transform) {
        PrintAllComponents(transform.gameObject);
    }

    internal static void PrintAllComponents(Component component) {
        PrintAllComponents(component.gameObject);
    }
    
    private static void PrintAllChildren(Transform transform, int depth, bool recursive = false, bool components = false) {
        if(components) {
            PrintAllComponents(transform.gameObject, depth + 1);
        }
        foreach(Transform child in transform) {
            Plugin.Log.LogWarning($"{new string(' ', depth * 2)}○ {child.name} {child.localPosition} {child.localRotation.eulerAngles} {child.localScale} {child.tag} {child.gameObject.layer}");
            if(recursive) {
                PrintAllChildren(child, depth + 1, recursive, components);
            }
        }
    }
    
    internal static void PrintAllChildren(Transform transform, bool recursive = false, bool components = false) {
        Plugin.Log.LogWarning($"Children of [{transform}]:");
        PrintAllChildren(transform, 1, recursive, components);
    }

    internal static void PrintAllChildren(GameObject gameObject, bool recursive = false, bool components = false) {
        PrintAllChildren(gameObject.transform, recursive, components);
    }

    internal static void PrintAllChildren(Component component, bool recursive = false, bool components = false) {
        PrintAllChildren(component.gameObject, recursive, components);
    }
    
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
        // TODO: it'd be good to make DebugUtil not require a reference to the assembly
        // consider adding a function which registers a custom formatter for a type
        Header(data);
        Log($"beat progress {data._beatProgressToStartOn}");
        Log($"duration {data._durationInBeats}");
        Log($"should ignore tempo {data._shouldIgnoreEnemyTempo}");
        Dump(data._animClip);
        var sprites = data._animationSprites;
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
    
    public static void Dump(SpriteRenderer spriteRenderer) {
        Header(spriteRenderer);
        if(spriteRenderer.sprite) Dump(spriteRenderer.sprite);
        Log($"color {spriteRenderer.color}");
        Log($"flipX {spriteRenderer.flipX}");
        Log($"flipY {spriteRenderer.flipY}");
        Log($"draw mode {spriteRenderer.drawMode}");
        Log($"size {spriteRenderer.size}");
        Log($"mask interaction {spriteRenderer.maskInteraction}");
        Log($"sort point {spriteRenderer.spriteSortPoint}");
        Log($"sorting layer {spriteRenderer.sortingLayerName} {spriteRenderer.sortingLayerID}");
        Log($"sorting order {spriteRenderer.sortingOrder}");
        Footer(spriteRenderer);
    }
    
    public static void Dump(Animation animation) {
        Header(animation);
        if(animation.clip) Dump(animation.clip);
        Log($"clip count {animation.GetClipCount()}");
        foreach(AnimationState state in animation) {
            Dump(state.clip);
            Log($"state {state.name}");
            Log($"length {state.length}");
            Log($"normalized speed {state.normalizedSpeed}");
            Log($"speed {state.speed}");
            Log($"time {state.time}");
            Log($"normalized time {state.normalizedTime}");
            Log($"weight {state.weight}");
            Log($"wrap mode {state.wrapMode}");
        }
        Log($"play automatically {animation.playAutomatically}");
        Log($"animate physics {animation.animatePhysics}");
        Log($"culling mode {animation.cullingType}");
        Footer(animation);
    }
    
    public static void Dump(RREnemy enemy) {
        Header(enemy);
        Dump(enemy._onBeatShadowSprite);
        Dump(enemy._halfBeatShadowSprite);
        Dump(enemy._otherBeatShadowSprite);
        Log($"default shadow color {enemy._defaultShadowColor}");
        Log($"vibe power shadow color {enemy._vibePowerShadowColor}");
        Log($"default shadow shader color {enemy._defaultShadowShaderColor}");
        Log($"vibe power shadow shader color {enemy._vibePowerShadowShaderColor}");
        Dump(enemy._resetAnimationClip);
        Log($"num frames to move towards action row {enemy._numFramesToMoveTowardsActionRow}");
        Log($"should override default movement curve {enemy._shouldOverrideDefaultMoveCurve}");
        Log($"movement curve {enemy._movementCurve}");
        Dump(enemy._movementAnimationData);
        Log($"should wrap around {enemy._shouldWrapAroundGrid}");
        Log($"percent to move {enemy._percentageThroughMovementToWrapAroundGrid}");
        Log($"percent to play animation {enemy._percentageThroughMovementToPlayGridLeavingAnimation}");
        Dump(enemy._leaveBoardOnLeftAnimationData);
        Dump(enemy._leaveBoardOnRightAnimationData);
        Dump(enemy._enterBoardOnLeftAnimationData);
        Dump(enemy._enterBoardOnRightAnimationData);
    }
}
