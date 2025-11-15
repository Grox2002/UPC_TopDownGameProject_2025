using UnityEngine;

public class Ysort : MonoBehaviour
{
    [SerializeField] private int sortMultiplier = 100; // Ajusta según tu escala
    private SpriteRenderer spriteRenderer;
    private float lastY;

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
        // Cuanto más bajo (menor Y), mayor sortingOrder => se dibuja encima
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * sortMultiplier);
        lastY = transform.position.y;
    }
}
