using UnityEngine;

[CreateAssetMenu(fileName = "HitData", menuName = "RiftEnemiesPlus/HitData")]
public class HitData : ScriptableObject {
    public float delay;
    public int offset;
    public Sprite[] sprites;
}
