using UnityEngine;

public class Ysort : MonoBehaviour
{
    [SerializeField] private int sortMultiplier = 100;
    private SpriteRenderer spriteRenderer;
    private float lastY;

    [Header("Sorting Offset Gizmo")]
    [SerializeField] private float yOffset = 0.5f;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        lastY = transform.position.y;
        UpdateOrder();
    }

    void LateUpdate()
    {
        if (Mathf.Abs(transform.position.y - lastY) > 0.001f)
            UpdateOrder();
    }

    void UpdateOrder()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y + yOffset) * sortMultiplier);
        lastY = transform.position.y;
    }

    
}
