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
            ____animationComponent.AddClip(data.AnimClip, data.AnimClip.name);
            ____shieldedMoveAnimData = data;
        }
        
        if(Config.AssetSwaps.BlueShields) {
            var obj = Plugin.Assets.LoadAsset<GameObject>("BlueShieldSkeletonAnim");
            var container = obj.GetComponent<AnimationDataContainer>();
            var data = container.animationData;
            ____animationComponent.AddClip(data.AnimClip, data.AnimClip.name);
            ____extraShieldMoveAnimData = data;
        }
    }
}
