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
    }

    private void CloseInventory()
    {
        InventoryPanel.SetActive(false);
    }
}
