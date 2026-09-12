using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryUIController : MonoBehaviour
{
    /** Component References **/

    [SerializeField]
    private FishInventory fishInventory;

    [SerializeField]
    private GameObject InventoryPanel;

    [SerializeField]
    private RectTransform ContentPanel;

    [SerializeField]
    private GameObject fishInventoryRowPrefab;

    /** Lifecycle Functions **/

    private void Update()
    {
        if (Keyboard.current.qKey.wasPressedThisFrame)
        {
            if (!InventoryPanel.activeSelf)
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
        InventoryPanel.SetActive(true);

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
        InventoryPanel.SetActive(false);
    }
}
