using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Statue : MonoBehaviour
{
    [SerializeField] private GameObject interactionUI; 
    [SerializeField] private Collider2D _detector;
    [SerializeField] private AudioSource _statueUsed;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Light2D _statueLight;

    private bool playerNearby = false;
    private bool used = false;
    private P_Health playerHealth;
    

    void Start()
    {
        
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    void Update()
    {
        if (!playerNearby || used) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            _statueUsed.Play();
            used = true;
            CheckPointManager.Instance.SetCheckpoint(_spawnPoint.position, playerHealth._currentHealth);
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
            //playerHealth = null;

            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void OnUsed()
    {
        
        if (interactionUI != null)
            interactionUI.SetActive(false);

        
        if (_statueLight != null)
            _statueLight.enabled = false;

        
        _detector.enabled = false;
    }
}
