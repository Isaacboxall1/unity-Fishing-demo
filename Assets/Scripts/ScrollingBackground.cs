using UnityEngine;
using UnityEngine.UI;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField]
    private Transform firstSprite;

    [SerializeField]
    private Transform secondSprite;

    [SerializeField]
    private float ScrollSpeed = 0.3f;

    private void Update()
    {
        float movement = ScrollSpeed * Time.deltaTime;

        firstSprite.localPosition += Vector3.left * movement;
        secondSprite.localPosition += Vector3.left * movement;
    }
}
