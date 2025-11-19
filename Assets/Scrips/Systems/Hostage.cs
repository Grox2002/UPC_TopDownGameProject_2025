using UnityEngine;

public class Hostage : MonoBehaviour
{
    public bool isSafe = false;
    private bool _canInteract = false;

    [SerializeField] private GameObject interactionUI;


    private void Start()
    {
        if (interactionUI != null)
            interactionUI.SetActive(false);
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
        HostageSystem.Instance.Savaged();
        
        gameObject.SetActive(false);
    }
}
