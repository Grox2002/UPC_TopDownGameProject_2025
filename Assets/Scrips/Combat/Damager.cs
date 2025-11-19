using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] private int _damage = 10;

    [SerializeField] private string targetTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(targetTag))
        {
            other.GetComponent<P_Health>().TakeDamage(_damage);
            other.GetComponent<P_Health>().StartCoroutine("ShrinkAndFall");
        }
        
    }
}
