using UnityEngine;
using UnityEngine.EventSystems;

public class Hostage : MonoBehaviour
{
    public string hostageId = "Hostage_1";
    public bool isSafe = false;
    private bool _canInteract = false;

    [SerializeField] private GameObject interactionUI;
    [SerializeField] private HostageSystem _hostageSystem;


    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
    }

    private void Awake()
    {
        if (PlayerPrefs.GetInt(hostageId, 0) == 1)
        {
            isSafe = true;
            gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (_canInteract && !isSafe && Input.GetKeyDown(KeyCode.E))
        {
            Save();
            interactionUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteract = true;
            if (!isSafe && interactionUI != null)
                interactionUI.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _canInteract = false;
            if (interactionUI != null)
                interactionUI.SetActive(false);
        }
    }

    private void Save()
    {
        isSafe = true;
        PlayerPrefs.SetInt(hostageId, 1);
        _hostageSystem.Savaged();
        
        gameObject.SetActive(false);
    }
}
