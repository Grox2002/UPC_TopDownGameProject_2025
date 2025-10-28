using UnityEngine;
using System.Collections;

public class DivinePunishment : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float followSpeed = 2f;

    [Header("Daño")]
    [SerializeField] private float damagePerSecond = 1000f;

    private Transform player;
    private P_Health playerHealth;
    private bool playerInside;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform; 
            playerHealth = playerObj.GetComponent<P_Health>();
        }
    }

    void Update()
    {
        if (player == null) return;

        // Seguir al jugador
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;

        // Aplicar daño constante mientras esté dentro
        if (playerInside && playerHealth != null)
        {
            playerHealth.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            playerHealth = other.GetComponent<P_Health>();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            playerHealth = null;
        }
    }
}
