using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    /** Component References **/

    [SerializeField]
    private FishInventory fishInventory;

    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private RectTransform ContentPanel;

    [SerializeField]
    private GameObject fishInventoryRowPrefab;

    [SerializeField]
    private FishingController fishingController;

    /** Lifecycle Functions **/

    private void Update()
    {
        if (inventoryPanel.activeSelf && !fishingController.CanOpenInventory)
        {
            CloseInventory();
            return;
        }

        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (!inventoryPanel.activeSelf)
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        inventoryPanel.SetActive(true);

        foreach (Transform child in ContentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var entry in fishInventory.Inventory)
        {
            GameObject instantiatedObject = Instantiate(fishInventoryRowPrefab, ContentPanel);

            FishInventoryRow inventoryRowComponent = instantiatedObject.GetComponent<FishInventoryRow>();

            inventoryRowComponent.UpdateInventoryDisplay(entry.Key, entry.Value);
        }
    }

    private void CloseInventory()
    {
        inventoryPanel.SetActive(false);
    }
}
