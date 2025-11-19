using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    //Variables

    
    public bool IsDialogueFinished { get; private set; }
    public bool isTyping { get; private set; }

    [Header("UI")]
    public GameObject dialogueBox;
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private float _typingSpeed = 0.02f;

    private string _currentSentence;
    private Queue<DialogueLine> lines = new Queue<DialogueLine>();

    private Coroutine typingCoroutine;
    private int currentPage = 1;

    [SerializeField] private P_Attack _p_Attack;
    [SerializeField] private P_Movement _p_Movement;

    void Awake()
    {

        dialogueBox.SetActive(false);
        IsDialogueFinished = true;
    }

    public void StartDialogue(DialogSO dialogue)
    {
 

        if (dialogue == null)
        {
            Debug.LogWarning("StartDialogue: diálogo null.");
            return;
        }

        IsDialogueFinished = false;
        dialogueBox.SetActive(true);


        lines.Clear();

        foreach (var line in dialogue.dialogueLines)
            lines.Enqueue(line);

        DisplayNextLine();
    }

    void Update()
    {
        if (!dialogueBox.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = null;

                _dialogueText.text = _currentSentence;  
                isTyping = false;

                
                _dialogueText.ForceMeshUpdate();
                currentPage = 1;
                _dialogueText.pageToDisplay = currentPage;

                return;
            }

            
            if (currentPage < _dialogueText.textInfo.pageCount)
            {
                currentPage++;
                _dialogueText.pageToDisplay = currentPage;
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    public void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        _nameText.text = line.characterName;
        _portraitImage.sprite = line.portrait;
        _currentSentence = line.sentence ?? "";

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeSentence(_currentSentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        _dialogueText.text = "";

        

        foreach (char letter in sentence)
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }

        isTyping = false;
        typingCoroutine = null;

        
        _dialogueText.ForceMeshUpdate();

        currentPage = 1;
        _dialogueText.pageToDisplay = currentPage;
    }

    public void EndDialogue()
    {
        dialogueBox.SetActive(false);
        IsDialogueFinished = true;
       
    }

}
