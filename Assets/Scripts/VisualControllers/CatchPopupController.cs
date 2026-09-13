using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CatchPopupController : MonoBehaviour
{
    [SerializeField]
    Image fishImage;

    [SerializeField]
    TMP_Text catchText;

    public void ShowCatch(FishDefinition fishDefinition)
    {
        gameObject.SetActive(true);
        fishImage.sprite = fishDefinition.Sprite;
        catchText.text = $"You caught a {fishDefinition.DisplayName}!";
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
