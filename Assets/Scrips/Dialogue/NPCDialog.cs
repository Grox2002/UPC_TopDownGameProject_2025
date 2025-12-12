using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    //[SerializeField] private DialogManager dialogManager;
    [SerializeField] private GameObject _interactPopup;

    public DialogSO initialDialogue;
    public DialogSO missionCompleteDialogue;

    private bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E) && !DialogManager.Instance.IsDialogueActive)
        {
            if (MissionManager.Instance.CryptaHostagesCompleted)
            {
                DialogManager.Instance.StartDialogue(missionCompleteDialogue);
            }
            else
            {
                DialogManager.Instance.StartDialogue(initialDialogue);
            }

            _interactPopup.SetActive(false);
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            _interactPopup.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            _interactPopup.SetActive(false);

            DialogManager.Instance.EndDialogue();
        }
    }
}
