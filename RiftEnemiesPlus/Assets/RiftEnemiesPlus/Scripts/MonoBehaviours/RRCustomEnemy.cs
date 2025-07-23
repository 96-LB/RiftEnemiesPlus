using System;
using RhythmRift;
using RhythmRift.Enemies;
using Shared.RhythmEngine;
using Unity.Mathematics;
using UnityEngine;


namespace RiftEnemiesPlus {
    public class RRCustomEnemy : RREnemy {

        public CustomEnemyDefinition Definition { get; private set; }
        public int CurrentHitNum { get; private set; }
        public HitData CurrentHit => Definition.hits != null && CurrentHitNum < Definition.hits.Length ? Definition.hits[CurrentHitNum] : new();
        public int2 DesiredGridPositionAfterHit { get; private set; }
        public float AnimationStartBeat { get; private set; }
        public CustomAnimation Animation { get; private set; }

        public override float ExpectedFollowUpActionTrueBeatNumber =>
            CurrentHit == null ? base.ExpectedFollowUpActionTrueBeatNumber : NextActionRowTrueBeatNumber + CurrentHit.delay;

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
            
            PlayCustomAnimation(CurrentHit.animation, enemyInitializationData.SpawnTrueBeatNumber, fmodTimeCapsule);
        }

        public override int2 GetDesiredGridPosition() {
            if(IsPerformingHitMovement) {
                return DesiredGridPositionAfterHit;
            }
            return base.GetDesiredGridPosition();
        }

        public override void PerformTakeDamageBehaviour(FmodTimeCapsule fmodTimeCapsule) {
            DesiredGridPositionAfterHit = new(CurrentGridPosition.x + CurrentHit.offset, RRGridView.HOME_ROW_COORD.y + Mathf.FloorToInt(CurrentHit.delay));
            CurrentHitNum++;
            base.PerformTakeDamageBehaviour(fmodTimeCapsule);
        }

        public override void UpdateState(FmodTimeCapsule fmodTimeCapsule) {
            base.UpdateState(fmodTimeCapsule);
            UpdateCustomAnimation(fmodTimeCapsule);
        }
        
        public void UpdateCustomAnimation(FmodTimeCapsule fmodTimeCapsule) {
            if(Animation != null && !IsDying && _spriteRenderer) {
                float time = fmodTimeCapsule.TrueBeatNumber - Mathf.Max(0, AnimationStartBeat);
                _spriteRenderer.sprite = Animation.GetSpriteAtTime(time);
                Plugin.Log.LogDebug($"Updating {AnimationStartBeat} - {time}, sprite: {_spriteRenderer.sprite.name ?? "null"}");
            }
        }

        public void PlayCustomAnimation(CustomAnimation animation, float startBeat, FmodTimeCapsule fmodTimeCapsule) {
            if(animation == null) {
                Plugin.Log.LogWarning("Tried to play a null custom animation.");
                return;
            }
            Animation = animation;
            AnimationStartBeat = startBeat;
            UpdateCustomAnimation(fmodTimeCapsule);
        }
    }
}
