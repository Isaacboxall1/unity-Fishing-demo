using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "Fishing/FishDefinition")]
public class FishDefinition : ScriptableObject
{

    [SerializeField]
    private string displayName;

    [SerializeField]
    private Sprite sprite;

    [SerializeField]
    private float fishMoveSpeed = 200f;

    [SerializeField]
    private float minTargetChangeDelay = 0.5f;

    [SerializeField]
    private float maxTargetChangeDelay = 1.5f;

    [SerializeField]
    private float progressGainRate = 0.25f;

    [SerializeField]
    private float progressLossRate = 0.15f;

    public string DisplayName => displayName;
    public Sprite Sprite => sprite;

    public float FishMoveSpeed => fishMoveSpeed;

    public float MaxTargetChangeDelay => maxTargetChangeDelay;

    public float MinTargetChangeDelay => minTargetChangeDelay;

    public float ProgressGainRate => progressGainRate;

    public float ProgressLossRate => progressLossRate;
}
