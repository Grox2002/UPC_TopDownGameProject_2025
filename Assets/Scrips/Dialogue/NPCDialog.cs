using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private DialogManager dialogManager;

    public DialogSO dialogue;
    private bool playerInRange;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            dialogManager.StartDialogue(dialogue);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Player entró en el trigger del NPC");
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
