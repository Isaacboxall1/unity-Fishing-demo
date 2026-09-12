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

    /** Private Variables **/

    private float catchBarVelocity = 0f;

    private float fishTargetY = 0f;

    private bool isMinigameActive = false;

    private float catchProgress = 0.5f;

    private FishDefinition currentFish;

    /** Lifecycle Functions **/

    void Update()
    {
        if (!isMinigameActive)
        {
            return;
        }

        UpdateCatchBar();
        UpdateFishPosition();
        UpdateCatchProgress();
    }

    /** Public Methods **/

    public float CatchProgress => catchProgress;

    public bool IsMinigameActive => isMinigameActive;

    public void StartMinigame(FishDefinition fishDefinition)
    {
        currentFish = fishDefinition;

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
        currentFish = null;
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

    private void UpdateFishPosition()
    {
        float currentFishY = fish.anchoredPosition.y;
        float updatedFishY = Mathf.MoveTowards(currentFishY, fishTargetY, currentFish.FishMoveSpeed * Time.deltaTime);
        fish.anchoredPosition = new Vector2(fish.anchoredPosition.x, updatedFishY);
    }
    private void UpdateCatchProgress()
    {
        if (IsFishInsideCatchBar())
        {
            catchProgress += currentFish.ProgressGainRate * Time.deltaTime;
        }
        else
        {
            catchProgress -= currentFish.ProgressLossRate * Time.deltaTime;
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

            float waitTime = UnityEngine.Random.Range(currentFish.MinTargetChangeDelay, currentFish.MaxTargetChangeDelay);

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
