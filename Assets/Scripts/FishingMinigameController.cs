using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections;
using System;

public class FishingMinigameController : MonoBehaviour
{
    /** Events **/

    public event Action<bool> MinigameFinished;

    /** Component References **/

    [SerializeField]
    private RectTransform fish;

    [SerializeField]
    private RectTransform catchBar;

    [SerializeField]
    private RectTransform fishingTrack;

    [SerializeField]
    private Image progressFill;

    /** Catch Bar Settings **/

    [SerializeField]
    private float upwardAcceleration = 700f;

    [SerializeField]
    private float gravity = 500f;

    [SerializeField]
    private float maxSpeed = 350f;

    /** Fish Settings **/

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

    /** Private Variables **/

    private float catchBarVelocity = 0f;

    private float fishTargetY = 0f;

    private bool isMinigameActive = false;

    private float catchProgress = 0.5f;

    /** Lifecycle Functions **/

    void Update()
    {
        if (!isMinigameActive)
        {
            return;
        }

        UpdateCatchBar();
        UpdateFish();
        UpdateCatchProgress();
    }

    /** Public Methods **/
    public void StartMinigame()
    {
        fish.anchoredPosition = new Vector2(0f, 0f);
        catchBar.anchoredPosition = new Vector2(0f, 0f);
        catchBarVelocity = 0f;
        catchProgress = 0.5f;
        progressFill.fillAmount = catchProgress;

        isMinigameActive = true;

        StartCoroutine(UpdateFishTarget());
    }

    public void StopMinigame()
    {
        isMinigameActive = false;
    }

    private void FinishMinigame(bool success)
    {
        StopMinigame();
        MinigameFinished?.Invoke(success);
    }

    /** Private Helpers **/
    private void UpdateCatchBar()
    {
        if (Keyboard.current.spaceKey.isPressed)
        {
            catchBarVelocity += upwardAcceleration * Time.deltaTime;
        }
        else
        {
            catchBarVelocity -= gravity * Time.deltaTime;
        }

        catchBarVelocity = Mathf.Clamp(catchBarVelocity, -maxSpeed, maxSpeed);

        float currentY = catchBar.anchoredPosition.y;
        float desiredYPosition = currentY + catchBarVelocity * Time.deltaTime;
        Vector2 catchBarYBounds = CalculateCatchBarYBounds();

        float newY = Mathf.Clamp(desiredYPosition, catchBarYBounds.x, catchBarYBounds.y);

        // hitting boundary
        if (newY == catchBarYBounds.x || newY == catchBarYBounds.y)
        {
            catchBarVelocity = 0f;
        }

        catchBar.anchoredPosition = new Vector2(catchBar.anchoredPosition.x, newY);
    }

    private void UpdateFish()
    {
        float currentFishY = fish.anchoredPosition.y;
        float updatedFishY = Mathf.MoveTowards(currentFishY, fishTargetY, fishMoveSpeed * Time.deltaTime);
        fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, updatedFishY);
    }
    private void UpdateCatchProgress()
    {
        if (IsFishInsideCatchBar())
        {
            catchProgress += progressGainRate * Time.deltaTime;
        }
        else
        {
            catchProgress -= progressLossRate * Time.deltaTime;
        }

        catchProgress = Mathf.Clamp(catchProgress, 0f, 1f);

        progressFill.fillAmount = catchProgress;

        if (catchProgress >= 1f)
        {
            FinishMinigame(true);
        }
        else if (catchProgress <= 0f)
        {
            FinishMinigame(false);
        }
    }

    private Vector2 CalculateCatchBarYBounds()
    {
        return CalculateImageYBounds(catchBar);
    }

    private Vector2 CalculateFishYBounds()
    {
        return CalculateImageYBounds(fish);
    }

    private Vector2 CalculateImageYBounds(RectTransform image)
    {
        float trackHeight = fishingTrack.rect.height / 2;
        float imageHeight = image.rect.height / 2;
        float maxY = trackHeight - imageHeight;
        float minY = -maxY;

        return new Vector2(minY, maxY);
    }

    private void ChooseFishTarget()
    {
        Vector2 fishBounds = CalculateFishYBounds();
        fishTargetY = UnityEngine.Random.Range(fishBounds.x, fishBounds.y);
    }

    private IEnumerator UpdateFishTarget()
    {
        while (isMinigameActive)
        {
            ChooseFishTarget();

            float waitTime = UnityEngine.Random.Range(minTargetChangeDelay, maxTargetChangeDelay);

            yield return new WaitForSeconds(waitTime);
        }
    }
    private bool IsFishInsideCatchBar()
    {
        float fishTop = fish.anchoredPosition.y + fish.rect.height / 2;
        float fishBottom = fish.anchoredPosition.y - fish.rect.height / 2;

        float catchBarTop = catchBar.anchoredPosition.y + catchBar.rect.height / 2;
        float catchBarBottom = catchBar.anchoredPosition.y - catchBar.rect.height / 2;

        return (fishTop >= catchBarBottom) && (fishBottom <= catchBarTop);
    }
}
