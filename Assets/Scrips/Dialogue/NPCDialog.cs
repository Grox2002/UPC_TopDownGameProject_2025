using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    public DialogSO dialogue;

    private bool playerInRange;
    private bool dialogStarted;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogStarted)
            {
                dialogStarted = true;
                dialogManager.StartDialogue(dialogue);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialogStarted = false;   
        }
    }
}
