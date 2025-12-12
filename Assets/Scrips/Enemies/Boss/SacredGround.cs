using UnityEngine;

public class SacredGround : MonoBehaviour
{
    [SerializeField] private float _damageInterval = 0.001f;
    [SerializeField] private float _damage;
    private P_Health playerHealth;
    private bool playerInside = false;
    private float timer = 0f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerHealth = other.GetComponent<P_Health>();
            playerInside = true;
           
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
            timer = 0f;
        }
    }

    private void Update()
    {
        if (playerInside && playerHealth != null)
        {
            timer += Time.deltaTime;
            while (timer >= _damageInterval)
            {
                playerHealth.TakeDamage(_damage);
                timer -= _damageInterval;
            }
        }
    }
}
