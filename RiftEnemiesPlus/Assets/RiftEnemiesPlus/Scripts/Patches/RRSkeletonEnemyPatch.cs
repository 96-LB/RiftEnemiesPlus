using HarmonyLib;
using RhythmRift.Enemies;
using UnityEngine;


namespace RiftEnemiesPlus.Patches {
    [HarmonyPatch(typeof(RRSkeletonEnemy), nameof(RRSkeletonEnemy.OnSpawn))]
    internal static class RRSkeletonEnemyPatch {
        static bool x = false;
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

            return;
            Plugin.Log.LogError("here we go");
            DebugUtil.Dump(__instance);

            if(!x) {
                x = true;
                DumpClip(__instance._movementAnimationData._animClip);
                DumpClip(__instance._resetAnimationClip);
                DumpClip(__instance._attackAnimationData._animClip);
                DumpClip(__instance._deathAnimationData._animClip);
                DumpClip(__instance._hitMovementAnimationData._animClip);
                DumpClip(__instance._portalInAnimationClip);
                DumpClip(__instance._portalOutAnimationClip);
                DumpClip(__instance._simpleEnterBoardOnLeftAnimationClip);
                DumpClip(__instance._simpleEnterBoardOnRightAnimationClip);
                DumpClip(__instance._simpleLeaveBoardOnLeftAnimationClip);
                DumpClip(__instance._simpleLeaveBoardOnRightAnimationClip);
                DumpClip(__instance._enterBoardOnLeftAnimationData._animClip);
                DumpClip(__instance._enterBoardOnRightAnimationData._animClip);
                DumpClip(__instance._leaveBoardOnLeftAnimationData._animClip);
                DumpClip(__instance._leaveBoardOnRightAnimationData._animClip);
            }
        }
        
        private static void DumpClip(AnimationClip clip) {
            var obj = new GameObject();
            var scalepoint = new GameObject("ScalePoint");
            scalepoint.transform.SetParent(obj.transform);
            var sprite = new GameObject("Sprite");
            sprite.transform.SetParent(scalepoint.transform);
            var renderer = sprite.AddComponent<SpriteRenderer>();
            obj.transform.position = Vector3.zero;
            obj.transform.rotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;
            var sampler = obj.AddComponent<AnimationSampler>();
            sampler.AddSampleFunctions(
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localPosition.x,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localPosition.y,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localPosition.z,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localRotation.eulerAngles.x,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localRotation.eulerAngles.y,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localRotation.eulerAngles.z,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localScale.x,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localScale.y,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").localScale.z,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").GetComponent<SpriteRenderer>().color.r,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").GetComponent<SpriteRenderer>().color.g,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").GetComponent<SpriteRenderer>().color.b,
                gameObject => gameObject.transform.Find("ScalePoint").Find("Sprite").GetComponent<SpriteRenderer>().color.a
            );
            sampler.SampleAnimation(clip);
        }
    }
}
