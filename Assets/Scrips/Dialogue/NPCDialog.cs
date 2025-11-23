using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;
    [SerializeField] private GameObject _intButton;

    public DialogSO dialogue;

    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!dialogManager.IsDialogueActive)
            {
                dialogManager.StartDialogue(dialogue);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            _intButton.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            _intButton.SetActive(false);

            dialogManager.EndDialogue();
        }
    }
}
