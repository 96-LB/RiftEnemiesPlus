using RhythmRift;
using RhythmRift.Enemies;
using Shared.RhythmEngine;
using UnityEngine;


namespace RiftEnemiesPlus {
    public class RRCustomEnemy : RREnemy {
        public override void Initialize(RREnemyInitializationData enemyInitializationData, AnimationCurve defaultMovementCurve, FmodTimeCapsule fmodTimeCapsule, bool shouldDisableMovementAnimations) {
            base.Initialize(enemyInitializationData, defaultMovementCurve, fmodTimeCapsule, shouldDisableMovementAnimations);
        }
    }
}
