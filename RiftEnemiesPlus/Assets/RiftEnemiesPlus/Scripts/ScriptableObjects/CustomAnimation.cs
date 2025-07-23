using System.Linq;
using UnityEngine;


namespace RiftEnemiesPlus {
    [CreateAssetMenu(fileName = "Animation", menuName = "RiftEnemiesPlus/Animation")]
    public class CustomAnimation : ScriptableObject {
        public Sprite[] sprites;
        public float[] spriteTimings;

        public float Duration => spriteTimings?.Sum() ?? 0f;

        public Sprite GetSpriteAtTime(float time) {
            if(sprites == null || spriteTimings == null) {
                Plugin.Log.LogError("CustomAnimation sprites or timings are null!");
                return null;
            }

            Sprite sprite = null;
            time %= Duration;
            for(int i = 0; i < Mathf.Min(sprites.Length, spriteTimings.Length); i++) {
                if(time <= 0) {
                    return sprite;
                }
                if(sprites[i] != null) {
                    sprite = sprites[i];
                }
                time -= spriteTimings[i];
            }
            return sprite;
        }
    }
}
