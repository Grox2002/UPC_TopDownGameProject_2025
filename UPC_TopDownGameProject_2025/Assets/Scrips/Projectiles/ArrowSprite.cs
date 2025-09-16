using UnityEngine;

public class ArrowSprite : MonoBehaviour
{
    public Transform arrowParent; 
    public float visualOffset = -45f; 

    void Update()
    {
        if (arrowParent == null) return;

        Vector2 direction = arrowParent.right;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + visualOffset);
    }


}
