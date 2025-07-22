using HarmonyLib;
using RhythmRift.Enemies;


namespace RiftEnemiesPlus.Patches {
    [HarmonyPatch(typeof(RRSkeletonEnemy), nameof(RRSkeletonEnemy.OnSpawn))]
    internal static class RRSkeletonEnemyPatch {
        public static void Postfix(RRSkeletonEnemy __instance) {
            AssetSwapper.TryAddSwap(
                __instance._shieldedMoveAnimData._animationSprites,
                Assets.Instance.redShieldSkeletonSprites,
                Config.AssetSwaps.RedShields
            );
            AssetSwapper.TryAddSwap(
                __instance._extraShieldMoveAnimData._animationSprites,
                Assets.Instance.blueShieldSkeletonSprites,
                Config.AssetSwaps.BlueShields
            );
            //DebugUtil.Dump(__instance);
        }
    }
}
