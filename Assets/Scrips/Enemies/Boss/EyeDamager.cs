using UnityEngine;

public class EyeDamager : MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<P_Health>().TakeDamage(_damage);
        }

    }
}

