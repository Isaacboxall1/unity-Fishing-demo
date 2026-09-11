using UnityEngine;

[CreateAssetMenu(fileName = "FishDefinition", menuName = "Fishing/FishDefinition")]
public class FishDefinition : ScriptableObject
{
    [SerializeField]
    private string displayName;

    [SerializeField]
    private Sprite sprite;

    public string DisplayName => displayName;
    public Sprite Sprite => sprite;
}
