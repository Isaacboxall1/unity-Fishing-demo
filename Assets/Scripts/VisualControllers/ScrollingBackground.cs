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

    private float spriteWidth;

    private void Awake()
    {
        SpriteRenderer spriteRenderer = firstSprite.GetComponent<SpriteRenderer>();

        spriteWidth = spriteRenderer.sprite.bounds.size.x * Mathf.Abs(firstSprite.localScale.x);

        secondSprite.localPosition = new Vector3(spriteWidth, firstSprite.localPosition.y, firstSprite.localPosition.z);
    }
    private void Update()
    {
        float movement = ScrollSpeed * Time.deltaTime;

        firstSprite.localPosition += Vector3.left * movement;
        secondSprite.localPosition += Vector3.left * movement;

        WrapSpriteIfNeeded(firstSprite, secondSprite);
        WrapSpriteIfNeeded(secondSprite, firstSprite);
    }

    private void WrapSpriteIfNeeded(Transform sprite, Transform otherSprite)
    {
        if (sprite.localPosition.x <= -spriteWidth)
        {
            sprite.localPosition += Vector3.right * spriteWidth * 2f;
        }
    }
}
