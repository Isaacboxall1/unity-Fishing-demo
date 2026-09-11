using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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

    private FishingState currentState = FishingState.Ready;

    private Animator animator;

    private GameObject biteIndicator;

    [SerializeField]
    private float minBiteDelay = 2f;

    [SerializeField]
    private float maxBiteDelay = 5f;

    [SerializeField]
    private GameObject biteIndicatorPrefab;

    [SerializeField]
    private Transform fishingPoint;

    [SerializeField]
    private FishingMinigameController minigameController;



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
        currentState = FishingState.WaitingForBite;
        animator.Play("Player_Fishing");
        StartCoroutine(WaitForBite());
    }

    private IEnumerator WaitForBite()
    {
        float waitTime = Random.Range(minBiteDelay, maxBiteDelay);

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
        Debug.Log("Starting fishing minigame");
        minigameController.gameObject.SetActive(true);
        minigameController.StartMinigame();
    }
}
