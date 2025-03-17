using HarmonyLib;
using RhythmRift.Enemies;


[HarmonyPatch(typeof(RRSkeletonEnemy), "OnSpawn")]
internal static class RRSkeletonEnemy {
    public static void Postfix(
        ref SpriteAnimationData ____shieldedMoveAnimData,
        ref SpriteAnimationData ____extraShieldMoveAnimData
    ) {
        AssetSwapper.TryAddSwap(
            ____shieldedMoveAnimData._animationSprites,
            Assets.Instance.redShieldSkeletonSprites,
            Config.AssetSwaps.RedShields
        );
        AssetSwapper.TryAddSwap(
            ____extraShieldMoveAnimData._animationSprites,
            Assets.Instance.blueShieldSkeletonSprites,
            Config.AssetSwaps.BlueShields
        );
    }
}
