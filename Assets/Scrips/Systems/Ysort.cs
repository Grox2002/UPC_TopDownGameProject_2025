using UnityEngine;

public class Ysort : MonoBehaviour
{
    [SerializeField] private int sortMultiplier = 100;
    private SpriteRenderer spriteRenderer;
    private float lastY;

    [Header("Sorting Offset Gizmo")]
    [SerializeField] private float yOffset = 0.5f;
    [SerializeField] private Color gizmoColor = Color.yellow;
    [SerializeField] private float gizmoSize = 0.05f;

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
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-(transform.position.y + yOffset) * sortMultiplier);
        lastY = transform.position.y;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // Punto exacto usado para el sorting
        Vector3 offsetPos = transform.position + new Vector3(0, yOffset, 0);

        Gizmos.DrawSphere(offsetPos, gizmoSize);

        // Línea guía para que sea más fácil verlo
        Gizmos.DrawLine(transform.position, offsetPos);
    }
}
