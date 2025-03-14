using HarmonyLib;
using RhythmRift.Enemies;
using UnityEngine;

[HarmonyPatch(typeof(RRSkeletonEnemy), "OnSpawn")]
internal static class TestPatch {
    public static void Postfix(
        ref SpriteAnimationData ____shieldedMoveAnimData,
        ref SpriteAnimationData ____extraShieldMoveAnimData
    ) {
        AssetSwapper.TryAddSwap(
            ____shieldedMoveAnimData.Field<Sprite[]>("_animationSprites"),
            Assets.Instance.redShieldSkeletonSprites,
            Config.AssetSwaps.RedShields
        );
        AssetSwapper.TryAddSwap(
            ____extraShieldMoveAnimData.Field<Sprite[]>("_animationSprites"),
            Assets.Instance.blueShieldSkeletonSprites,
            Config.AssetSwaps.BlueShields
        );
    }
}
