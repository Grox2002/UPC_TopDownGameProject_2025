using UnityEngine;
using UnityEngine.UI;

public class CrossHairUI : MonoBehaviour
{
    private RectTransform rectTransform;
    [SerializeField] private Image crosshairImage;

    private Vector3 defaultScale;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        defaultScale = transform.localScale;
    }

    private void Update()
    {
        rectTransform.position = Input.mousePosition;
    }

    // --- Extras opcionales para feedback visual ---

    public void FlashRed()
    {
        if (crosshairImage != null)
            crosshairImage.color = Color.red;
    }

    public void ResetColor()
    {
        if (crosshairImage != null)
            crosshairImage.color = Color.white;
    }

    public void Pulse(float amount)
    {
        transform.localScale = defaultScale * (1f + amount);
    }

    public void ResetScale()
    {
        transform.localScale = defaultScale;
    }
}
