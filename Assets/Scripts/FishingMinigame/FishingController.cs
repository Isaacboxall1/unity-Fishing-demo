using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

enum FishingState
{
    Ready,
    WaitingForBite,
    FishHooked,
    Minigame,
    Reeling,
    ShowingCatch
}


public class FishingController : MonoBehaviour
{
    /** Member Variables **/

    [SerializeField]
    private float minBiteDelay = 2f;

    [SerializeField]
    private float maxBiteDelay = 5f;

    [SerializeField]
    private float reelingDuration = 1f;

    [SerializeField]
    private float biteWindowDuration = 1.5f;

    [SerializeField]
    private FishDefinition[] fishPool;

    [SerializeField]
    private GameObject biteIndicatorPrefab;

    [SerializeField]
    private Transform fishingPoint;

    [SerializeField]
    private FishingMinigameController minigameController;

    [SerializeField]
    private CatchPopupController catchPopupController;

    [SerializeField]
    private FishingLineController fishingLineController;

    [SerializeField]
    private FishingUIController fishingUIController;

    /** Private Variables **/

    private FishingState currentState = FishingState.Ready;

    private Animator animator;

    private FishInventory fishInventory;

    private GameObject biteIndicator;

    private FishDefinition currentFish;

    private Coroutine biteWindowCoroutine;

    /** Lifecycle Functions **/

    private void Update()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            HandleInteract();
        }
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        fishInventory = GetComponent<FishInventory>();
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (biteIndicator != null)
        {
            Destroy(biteIndicator);
            biteIndicator = null;
        }

        minigameController.MinigameFinished -= HandleMinigameFinished;
    }

    /** Public Methods **/

    public bool CanOpenInventory => currentState == FishingState.Ready;

    /** Private Helpers **/

    private void HandleInteract()
    {
        switch (currentState)
        {
            case FishingState.Ready:
                {
                    StartFishing();
                    break;
                }
            case FishingState.WaitingForBite:
                {
                    break;
                }
            case FishingState.FishHooked:
                {
                    StopCoroutine(biteWindowCoroutine);
                    biteWindowCoroutine = null;
                    StartMinigame();
                    break;
                }
            case FishingState.ShowingCatch:
                {
                    FinishCatch();
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    private void StartFishing()
    {
        if (!ChooseRandomFish())
        {
            return;
        }

        fishingUIController.HideInteractionPrompt();
        fishingLineController.ShowLine();
        currentState = FishingState.WaitingForBite;
        animator.Play("Player_Fishing");
        StartCoroutine(WaitForBite());
    }

    private IEnumerator WaitForBite()
    {
        float waitTime = UnityEngine.Random.Range(minBiteDelay, maxBiteDelay);

        yield return new WaitForSeconds(waitTime);

        HandleFishBite();
    }

    private void HandleFishBite()
    {
        fishingUIController.ShowCatchPrompt();

        currentState = FishingState.FishHooked;        

        biteIndicator = Instantiate(biteIndicatorPrefab, fishingPoint.position, Quaternion.identity);

        biteWindowCoroutine = StartCoroutine(WaitForHookInput());
    }

    private void StartMinigame()
    {
        if (biteIndicator != null)
        {
            Destroy(biteIndicator);
            biteIndicator = null;
        }

        fishingUIController.ShowMinigamePrompt();

        currentState = FishingState.Minigame;

        minigameController.MinigameFinished += HandleMinigameFinished;

        minigameController.gameObject.SetActive(true);
        minigameController.StartMinigame(currentFish);

    }
    private void HandleMinigameFinished(bool success)
    {
        minigameController.MinigameFinished -= HandleMinigameFinished;
        minigameController.gameObject.SetActive(false);

        fishingLineController.HideLine();
        fishingUIController.HideInteractionPrompt();
        
        if (success)
        {
            fishInventory.AddFish(currentFish);
            currentState = FishingState.Reeling;
            StartCoroutine(EnterCatchState());
        }
        else
        {
            SetPlayerReady();
        }
    }

    private bool ChooseRandomFish()
    {
        if (fishPool.Length == 0)
        {
            Debug.LogError("Fish Pool Is Empty - Aborting ChooseRandomFish");
            return false;
        }

        int chosenIndex = UnityEngine.Random.Range(0, fishPool.Length);
        currentFish = fishPool[chosenIndex];
        return true;
    }

    private IEnumerator EnterCatchState()
    {
        animator.Play("Player_Reeling");

        yield return new WaitForSeconds(reelingDuration);

        catchPopupController.ShowCatch(currentFish);
        currentState = FishingState.ShowingCatch;
    }

    private void FinishCatch()
    {
        catchPopupController.Hide();
        SetPlayerReady();
    }

    private IEnumerator WaitForHookInput()
    {
        yield return new WaitForSeconds(biteWindowDuration);

        if (currentState != FishingState.FishHooked)
        {
            yield break;
        }

        HandleMissedBite();
    }

    private void HandleMissedBite()
    {
        SetPlayerReady();
    }

    private void SetPlayerReady()
    {
        if (biteIndicator != null)
        {
            Destroy(biteIndicator);
            biteIndicator = null;
        }

        fishingUIController.ShowCastPrompt();
        fishingLineController.HideLine();
        animator.Play("Player_Idle");
        currentState = FishingState.Ready;
        currentFish = null;
        biteWindowCoroutine = null;
    }
}
