using UnityEngine;
using System.Collections;

public class DivinePunishment : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float followSpeed = 2f;

    [Header("Daño por frame")]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _damageInterval = 0.001f;
    private float timer = 0f;

    private Transform player;
    private P_Health playerHealth;
    private bool playerInside;

    [SerializeField] private AudioSource _laserSound;

    void Start()
    {
        _laserSound.Play();

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

        
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;


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

    
}
