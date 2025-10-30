using UnityEngine;

public class BossDashDamager : MonoBehaviour
{
    [SerializeField] private float _damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<P_Health>().TakeDamage(_damage);
        }
    }
}
