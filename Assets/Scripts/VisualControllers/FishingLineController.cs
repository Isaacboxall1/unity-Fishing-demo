using UnityEngine;

public class FishingLineController : MonoBehaviour
{
    /** Component References **/

    [SerializeField]
    private Transform rodTip;

    [SerializeField]
    private Transform fishingPointClose;

    [SerializeField]
    private Transform fishingPointFar;

    [SerializeField]
    private FishingMinigameController fishingMinigameController;

    [SerializeField]
    private float sagAmount = 0.5f;

    /** Member Variables **/

    private LineRenderer lineRenderer;

    /** Lifecycle Functions **/

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        UpdateLine();
    }

    /** Public Methods **/

    public void ShowLine()
    {
        gameObject.SetActive(true);
    }

    public void HideLine()
    {
        gameObject.SetActive(false);
    }

    /** Private Helpers **/

    private void UpdateLine()
    {
        lineRenderer.SetPosition(0, rodTip.position);
        lineRenderer.SetPosition(1, CalculateBendPoint());
        lineRenderer.SetPosition(2, GetFishingPointPosition());
    }

    private Vector2 CalculateBendPoint()
    {
        Vector2 midPoint = Vector2.Lerp(rodTip.position, GetFishingPointPosition(), 0.5f);
        midPoint += Vector2.down * sagAmount;
        return midPoint;
    }

    private Vector2 GetFishingPointPosition()
    {
        float lerpAmount = 0.5f;

        if (fishingMinigameController.IsMinigameActive)
        {
            lerpAmount = fishingMinigameController.CatchProgress;
        }
        
        return Vector2.Lerp(fishingPointFar.position, fishingPointClose.position, lerpAmount);
    }
}
