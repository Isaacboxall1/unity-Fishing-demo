using UnityEngine;

public class FishingLineController : MonoBehaviour
{
    /** Component References **/

    [SerializeField]
    private Transform rodTip;

    [SerializeField]
    private Transform fishingPoint;

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
        lineRenderer.SetPosition(2, fishingPoint.position);
    }

    private Vector2 CalculateBendPoint()
    {
        Vector2 midPoint = Vector2.Lerp(rodTip.position, fishingPoint.position, 0.5f);
        midPoint += Vector2.down * sagAmount;
        return midPoint;
    }
}
