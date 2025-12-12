using UnityEngine;

public class Healer : MonoBehaviour
{
    [SerializeField] private int _healAmount;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<P_Health>().Heal(_healAmount); 
        }

        Destroy(gameObject);
    }
}
