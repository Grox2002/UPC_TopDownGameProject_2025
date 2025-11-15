using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Healer : MonoBehaviour
{
    [Header("Configuración de curación")]
    [SerializeField] private float healAmount = 25f;

    [Header("Referencias")]
    [SerializeField] private GameObject interactionUI; 
    [SerializeField] private Collider2D _detector;

    private bool playerNearby = false;
    private bool used = false;
    private P_Health playerHealth;
    [SerializeField] private Light2D statueLight;

    void Start()
    {
        
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (!playerNearby || playerHealth == null || used) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            playerHealth.Heal(healAmount);
            used = true;
            Debug.Log("El jugador fue curado por la estatua.");
            OnUsed();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (used) return;

        if (collision.CompareTag("Player"))
        {
            playerHealth = collision.GetComponent<P_Health>();

            playerNearby = true;
            
            interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerNearby = false;
            playerHealth = null;

            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void OnUsed()
    {
        
        if (interactionUI != null)
            interactionUI.SetActive(false);

        
        if (statueLight != null)
            statueLight.enabled = false;

        
        _detector.enabled = false;
    }
}
