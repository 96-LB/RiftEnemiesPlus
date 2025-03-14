using System.ComponentModel;
using HarmonyLib;
using RhythmRift.Enemies;
using UnityEngine;

[HarmonyPatch(typeof(RRSkeletonEnemy), "OnSpawn")]
internal static class TestPatch {
    private static void Postfix(
        RRSkeletonEnemy __instance,
        Animation ____animationComponent,
        ref SpriteAnimationData ____shieldedMoveAnimData,
        ref SpriteAnimationData ____extraShieldMoveAnimData
    ) {
        
        if(!__instance.GetComponent<AssetSwapper>()) {
            var assetSwapper = __instance.gameObject.AddComponent<AssetSwapper>();
            assetSwapper.Initialize(
                ____shieldedMoveAnimData.Field<Sprite[]>("_animationSprites"),
                Assets.Instance.redShieldSkeletonSprites,
                Config.AssetSwaps.RedShields
            );
            
            var assetSwapper2 = __instance.gameObject.AddComponent<AssetSwapper>();
            assetSwapper2.Initialize(
                ____extraShieldMoveAnimData.Field<Sprite[]>("_animationSprites"),
                Assets.Instance.blueShieldSkeletonSprites,
                Config.AssetSwaps.BlueShields
            );
        }
    }
}
