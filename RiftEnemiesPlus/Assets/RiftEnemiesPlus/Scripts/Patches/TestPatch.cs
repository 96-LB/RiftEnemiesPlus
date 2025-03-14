using System.Reflection;
using HarmonyLib;
using RhythmRift.Enemies;
using Shared.RhythmEngine;
using UnityEngine;

using P = RhythmRift.Enemies.RRSkeletonEnemy;

[HarmonyPatch(typeof(P), nameof(P.OnSpawn))]
internal static class TestPatch {
    private static void Postfix(
        Animation ____animationComponent,
        ref SpriteAnimationData ____shieldedMoveAnimData,
        ref SpriteAnimationData ____extraShieldMoveAnimData
    ) {
        if(Config.AssetSwaps.RedShields) {
            var obj = Plugin.Assets.LoadAsset<GameObject>("RedShieldSkeletonAnim");
            var container = obj.GetComponent<AnimationDataContainer>();
            var data = container.animationData;
            
            data.AnimClip.legacy = true;
            data.AnimClip.wrapMode = WrapMode.Loop;
            ____animationComponent.AddClip(data.AnimClip, data.AnimClip.name);
            
            ____shieldedMoveAnimData = data;
        }
        
        if(Config.AssetSwaps.BlueShields) {
            var obj = Plugin.Assets.LoadAsset<GameObject>("BlueShieldSkeletonAnim");
            var container = obj.GetComponent<AnimationDataContainer>();
            var data = container.animationData;
            
            data.AnimClip.legacy = true;
            data.AnimClip.wrapMode = WrapMode.Loop;
            ____animationComponent.AddClip(data.AnimClip, data.AnimClip.name);
            
            ____extraShieldMoveAnimData = data;
        }
    }
}


[HarmonyPatch(typeof(RREnemy), nameof(RREnemy.UpdateAnimations))]
internal static class TestPatch2 {
    
    static P instance;
    private static void Postfix(
        RREnemy __instance,
        Animation ____animationComponent,
        SpriteAnimationData ____currentSpriteAnimData,
        FmodTimeCapsule fmodTimeCapsule
    ) {
        if(instance == null && __instance is P p && p.Field<int>("_currentShieldHealth") > 0) {
            instance = p;
        }
        if(instance == null || instance != __instance) {
            return;
        }
        Plugin.Log.LogInfo($"{____animationComponent[____currentSpriteAnimData.AnimClip.name].normalizedTime}, {instance.transform.rotation.eulerAngles.x}, {instance.transform.rotation.eulerAngles.y}, {instance.transform.rotation.eulerAngles.z}");
    }
}
