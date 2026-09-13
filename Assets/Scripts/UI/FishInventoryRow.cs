using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishInventoryRow : MonoBehaviour
{
    [SerializeField]
    private Image fishIcon;

    [SerializeField]
    private TMP_Text fishName;

    [SerializeField]
    private TMP_Text quantity;

    public void UpdateInventoryDisplay(FishDefinition fishDefinition, int fishQuantity)
    {
        fishIcon.sprite = fishDefinition.Sprite;

        fishName.text = fishDefinition.DisplayName;

        quantity.text = fishQuantity.ToString();
    }
}
