using RhythmRift;
using RhythmRift.Enemies;
using Shared.RhythmEngine;
using Unity.Mathematics;
using UnityEngine;


namespace RiftEnemiesPlus {
    public class RRCustomEnemy : RREnemy {

        public CustomEnemyDefinition Definition { get; private set; }
        public int CurrentHitNum { get; private set; }
        public HitData CurrentHit => Definition.GetHit(CurrentHitNum);
        public int2 DesiredGridPositionAfterHit { get; private set; }
        public float AnimationStartBeat { get; private set; }
        public CustomAnimation Animation { get; private set; }
        public float TempoAdjustment { get; set; }

        public override float ExpectedFollowUpActionTrueBeatNumber => float.PositiveInfinity; // audio sounds are kinda messed up for followup sounds
            //CurrentHit == null ? base.ExpectedFollowUpActionTrueBeatNumber : NextActionRowTrueBeatNumber + CurrentHit.delay;

        public override void Initialize(RREnemyInitializationData enemyInitializationData, AnimationCurve defaultMovementCurve, FmodTimeCapsule fmodTimeCapsule, bool shouldDisableMovementAnimations) {
            foreach(var enemy in Assets.Instance.enemies) {
                if(enemy.id == enemyInitializationData.EnemyDefinition.Id) {
                    Definition = enemy;
                    break;
                }
            }
            if(Definition == null) {
                Plugin.Log.LogError($"Failed to find custom enemy definition with ID {enemyInitializationData.EnemyDefinition.Id}.");
                return;
            }

            base.Initialize(enemyInitializationData, defaultMovementCurve, fmodTimeCapsule, shouldDisableMovementAnimations);

            CurrentHitNum = 0;
            AnimationStartBeat = 0f;
            Animation = null;

            PlayCustomAnimation(CurrentHit.movementAnimation, enemyInitializationData.SpawnTrueBeatNumber, fmodTimeCapsule);
        }

        public override int2 GetDesiredGridPosition() {
            if(IsSnappingToActionRow) {
                return _actionRowMoveTargetGridPosition;
            }
            if(IsPerformingHitMovement) {
                Plugin.Log.LogInfo($"Getting desired grid position for {gameObject.GetInstanceID()} after hit: {DesiredGridPositionAfterHit}");
                return DesiredGridPositionAfterHit;
            }
            return base.GetDesiredGridPosition();
        }

        public override void PerformTakeDamageBehaviour(FmodTimeCapsule fmodTimeCapsule) {
            DesiredGridPositionAfterHit = new(CurrentGridPosition.x + CurrentHit.offset, RRGridView.HOME_ROW_COORD.y + Mathf.FloorToInt(CurrentHit.delay));
            Plugin.Log.LogInfo($"Updating desired grid position for {gameObject.GetInstanceID()} after damage: {DesiredGridPositionAfterHit}");

            TempoAdjustment = CurrentHit.delay % 1f;
            base.PerformTakeDamageBehaviour(fmodTimeCapsule);
            TempoAdjustment = 0;
            CurrentHitNum++;
            PlayCustomAnimation(CurrentHit.movementAnimation, LastUpdateTrueBeatNumber, fmodTimeCapsule);
        }

        public override float GetUpdateTempoInBeats(bool shouldApplyBurnSpeedUp = true) {
            float num = TempoAdjustment + 1; // TODO: add to hitdata
            if(shouldApplyBurnSpeedUp && HasStatusEffectActive(RREnemyStatusEffect.Burning)) {
                num *= _burningSpeedUpdateRateOverride == 0f ? 0.5f : _burningSpeedUpdateRateOverride;
            }

            return num;
        }

        public override void UpdateState(FmodTimeCapsule fmodTimeCapsule) {
            base.UpdateState(fmodTimeCapsule);
            UpdateCustomAnimation(fmodTimeCapsule);
        }

        public override void PlayDeathAnimation(FmodTimeCapsule fmodTimeCapsule) {
            base.PlayDeathAnimation(fmodTimeCapsule);
            PlayCustomAnimation(Definition.deathAnimation, fmodTimeCapsule.TrueBeatNumber, fmodTimeCapsule);
        }

        public override void PlayAttackAnimation(FmodTimeCapsule fmodTimeCapsule) {
            base.PlayAttackAnimation(fmodTimeCapsule);
            PlayCustomAnimation(CurrentHit.attackAnimation, fmodTimeCapsule.TrueBeatNumber, fmodTimeCapsule);
        }

        public void UpdateCustomAnimation(FmodTimeCapsule fmodTimeCapsule) {
            if(Animation == null || !_spriteRenderer) {
                return;
            }
            float time = fmodTimeCapsule.TrueBeatNumber - Mathf.Max(0, AnimationStartBeat);
            time /= CurrentHit.animationSpeed;
            _spriteRenderer.sprite = Animation.GetSpriteAtTime(time);
        }

        public void PlayCustomAnimation(CustomAnimation animation, float startBeat, FmodTimeCapsule fmodTimeCapsule) {
            if(animation == null) {
                Plugin.Log.LogWarning("Tried to play a null custom animation.");
            }
            Animation = animation;
            AnimationStartBeat = startBeat;
            UpdateCustomAnimation(fmodTimeCapsule);
        }

        public override void PerformBeatActions(FmodTimeCapsule fmodTimeCapsule) {
            TempoAdjustment = IsHitWindowActive ? CurrentHit.delay % 1f : 0f;
            base.PerformBeatActions(fmodTimeCapsule);
            TempoAdjustment = 0f;
        }
    }
}
