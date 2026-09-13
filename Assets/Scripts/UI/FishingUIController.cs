using TMPro;
using UnityEngine;

public class FishingUIController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text interactionPrompt;

    [SerializeField]
    private TMP_Text statusMessage;

    [SerializeField]
    private TMP_Text inventoryPrompt;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowCastPrompt()
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = "Press E to cast";
    }

    public void ShowCatchPrompt()
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = "Press E to catch";
    }

    public void ShowMinigamePrompt()
    {
        interactionPrompt.gameObject.SetActive(true);
        interactionPrompt.text = "Press and hold space to fish!";
    }

    public void HideInteractionPrompt()
    {
        interactionPrompt.gameObject.SetActive(false);
    }
}
