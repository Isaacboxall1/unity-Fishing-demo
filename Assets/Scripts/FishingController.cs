using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System;

enum FishingState
{
    Ready,
    WaitingForBite,
    FishHooked,
    Minigame,
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
    private FishDefinition[] fishPool;

    [SerializeField]
    private GameObject biteIndicatorPrefab;

    [SerializeField]
    private Transform fishingPoint;

    [SerializeField]
    private FishingMinigameController minigameController;

    /** Private Variables **/

    private FishingState currentState = FishingState.Ready;

    private Animator animator;

    private FishInventory fishInventory;

    private GameObject biteIndicator;

    private FishDefinition currentFish;

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
                    StartMinigame();
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
        Debug.Log("Fish Hooked!");
        currentState = FishingState.FishHooked;        

        biteIndicator = Instantiate(biteIndicatorPrefab, fishingPoint.position, Quaternion.identity);
    }

    private void StartMinigame()
    {
        if (biteIndicator != null)
        {
            Destroy(biteIndicator);
            biteIndicator = null;
        }

        minigameController.MinigameFinished += HandleMinigameFinished;

        minigameController.gameObject.SetActive(true);
        minigameController.StartMinigame();

    }
    private void HandleMinigameFinished(bool success)
    {
        minigameController.MinigameFinished -= HandleMinigameFinished;
        minigameController.gameObject.SetActive(false);
        
        if (success)
        {
            fishInventory.AddFish(currentFish);
            Debug.Log(currentFish.DisplayName + " Caught! You now have: " + fishInventory.GetQuantity(currentFish));
            animator.Play("Player_Hooked");
            currentState = FishingState.Ready;
            currentFish = null;
        }
        else
        {
            Debug.Log("Fish Escaped!");
            animator.Play("Player_Idle");
            currentState = FishingState.Ready;
            currentFish = null;
        }
    }

    private bool ChooseRandomFish()
    {
        if (fishPool.Length == 0)
        {
            Debug.LogError("Fish Pool Is Empty - Aborting ChooseRandomFish");
            return false;
        }

        Int32 chosenIndex = UnityEngine.Random.Range(0, fishPool.Length);
        currentFish = fishPool[chosenIndex];
        return true;
    }
}
