using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    //Variables
    public bool IsDialogueFinished { get; private set; }
    public bool isTyping = false;
    public GameObject dialogueBox;

    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _dialogueText;
    [SerializeField] private Image _portraitImage;
    [SerializeField] private float _typingSpeed = 0.02f;   

    private string _currentSentence;
    private Queue<DialogueLine> lines;        

    //Metodos
    void Awake()
    {
  

        lines = new Queue<DialogueLine>();
        dialogueBox.SetActive(false);
        IsDialogueFinished = true;
    }

    public void StartDialogue(DialogSO dialogue)
    {
        dialogueBox.SetActive(true);
        lines.Clear();

        foreach (var line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }

        DisplayNextLine();
    }

    void Update()
    {
        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                StopAllCoroutines();
                _dialogueText.text = _currentSentence;
                isTyping = false;
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
        _currentSentence = line.sentence;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeSentence(_currentSentence));
    }
    private Coroutine typingCoroutine;
    IEnumerator TypeSentence(string sentence)
    {
        isTyping = true;
        _dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            _dialogueText.text += letter;
            yield return new WaitForSeconds(_typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        dialogueBox.SetActive(false);
        IsDialogueFinished = true;
    }

    
}
