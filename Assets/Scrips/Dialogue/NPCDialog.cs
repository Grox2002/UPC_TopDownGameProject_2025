using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class NPCDialog : MonoBehaviour
{
    [SerializeField] private GameObject _interactPopup;

    public DialogSO initialDialogue;
    public DialogSO missionCompleteDialogue;

    private bool playerInRange;
    

    void Start()
    {
        _interactPopup.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            // Solo iniciamos el diálogo si no hay uno activo
            if (!DialogManager.Instance.IsDialogueActive)
            {
                StartDialogue();
            }
            else
            {
                // Si ya hay diálogo activo, avisamos al DialogManager
                DialogManager.Instance.OnInteractKeyPressed();
            }
        }
    }

    private void StartDialogue()
    {
        if (MissionManager.Instance.CryptaHostagesCompleted)
            DialogManager.Instance.StartDialogue(missionCompleteDialogue);
        else
            DialogManager.Instance.StartDialogue(initialDialogue);
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

            if (DialogManager.Instance.IsDialogueActive)
                DialogManager.Instance.EndDialogue();
        }
    }
}
