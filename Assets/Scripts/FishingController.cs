using UnityEngine;
using UnityEngine.InputSystem;

enum FishingState
{
    Ready,
    WaitingForBite
}

public class FishingController : MonoBehaviour
{
    void Update()
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

    private void HandleInteract()
    {
        switch (currentState)
        {
            case FishingState.Ready:
                {
                    currentState = FishingState.WaitingForBite;
                    animator.Play("Player_Fishing");
                    break;
                }
            case FishingState.WaitingForBite:
                {
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    FishingState currentState = FishingState.Ready;

    private Animator animator;
}
