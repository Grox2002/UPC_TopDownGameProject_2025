using UnityEngine;

public class SacredGround : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private float damagePerSecond = 20f;
    [SerializeField] private string playerTag = "Player";

    private P_Health playerHealth;
    private bool playerInside = false;

    void Update()
    {
        // Aplica daño constante mientras el jugador esté dentro
        if (playerInside && playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = true;
            playerHealth = other.GetComponent<P_Health>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInside = false;
            playerHealth = null;
        }
    }
}
