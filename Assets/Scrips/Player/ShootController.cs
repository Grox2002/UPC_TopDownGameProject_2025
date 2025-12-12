using UnityEngine;
using UnityEngine.UIElements;


public class ShootController : MonoBehaviour
{
    public Transform weaponHolder;
    
    void LateUpdate()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)transform.position;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        weaponHolder.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}


