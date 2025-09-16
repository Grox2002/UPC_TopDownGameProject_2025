using UnityEngine;

public class ColliderDamager : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<E_Health>().TakeDamage(damage);
            
        }
    }
}
