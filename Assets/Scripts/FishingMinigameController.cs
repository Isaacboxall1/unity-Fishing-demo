using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FishingMinigameController : MonoBehaviour
{
    /** Member Variables **/

    [SerializeField]
    private RectTransform fish;

    [SerializeField]
    private RectTransform catchBar;

    [SerializeField]
    private RectTransform fishingTrack;

    [SerializeField]
    private Image progressFill;

    [SerializeField]
    private float upwardAcceleration = 700f;

    [SerializeField]
    private float gravity = 500f;

    [SerializeField]
    private float maxSpeed = 350f;

    private float catchBarVelocity = 0f;

    /** Lifecycle Functions **/

    void Update()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            catchBarVelocity += Mathf.Clamp(upwardAcceleration * Time.deltaTime, 0, maxSpeed);
        }
        else
        {
            catchBarVelocity -= Mathf.Clamp(gravity * Time.deltaTime, 0, maxSpeed);
        }

        UpdateCatchBar();
    }

    /** Public Methods **/
    public void StartMinigame()
    {
        fish.anchoredPosition = new Vector2(0f, 0f);
        catchBar.anchoredPosition = new Vector2(0f, 0f);
        progressFill.fillAmount = 0.5f;
        catchBarVelocity = 0f;
    }

    public void StopMinigame()
    {

    }

    /** Private Helpers **/

    public Vector2 calculateCatchBarYBounds()
    {
        float trackHeight = fishingTrack.rect.height / 2;
        float catchBarHeight = catchBar.rect.height / 2;
        float maxY = trackHeight - catchBarHeight;
        float minY = -maxY;

        return new Vector2(minY, maxY);
    }

    private void UpdateCatchBar()
    {
        float currentY = catchBar.anchoredPosition.y;
        float desiredYPosition = currentY + catchBarVelocity * Time.deltaTime;
        Vector2 catchBarYBounds = calculateCatchBarYBounds();

        float newY = Mathf.Clamp(desiredYPosition, catchBarYBounds.x, catchBarYBounds.y);

        // hitting boundary
        if (newY == catchBarYBounds.x || newY == catchBarYBounds.y)
        {
            catchBarVelocity = 0f;
        }

        catchBar.anchoredPosition = new Vector2(catchBar.anchoredPosition.x, newY);
    }
}
