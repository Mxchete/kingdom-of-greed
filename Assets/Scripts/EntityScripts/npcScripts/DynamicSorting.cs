using UnityEngine;

public class DynamicSorting : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Set the Order in Layer based on the Y-position (lower Y means further back)
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -10);
    }
}