using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialogueBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private float typingSpeed = 0.02f;

    [SerializeField] private AudioSource _tipingSource;

    private Queue<DialogueLine> lines = new Queue<DialogueLine>();
    private Coroutine typingCoroutine;
    private string currentSentence;

    public bool IsDialogueActive { get; private set; }
    public bool IsTyping { get; private set; }

    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    public void StartDialogue(DialogSO dialogue)
    {
        if (dialogue == null) return;

        StartCoroutine(StartDialogueDelayed(dialogue));
    }
    private IEnumerator StartDialogueDelayed(DialogSO dialogue)
    {
        yield return null; 
                           
        IsDialogueActive = true;
        dialogueBox.SetActive(true);
        dialogueText.text = "";
        lines.Clear();
        foreach (var l in dialogue.dialogueLines)
            lines.Enqueue(l);

        DisplayNextLine();
    }

    private void Update()
    {
        if (!IsDialogueActive) return;

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsTyping)
            {
                SkipTyping();
            }
            else
            {
                DisplayNextLine();
            }
        }
    }

    private void DisplayNextLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueLine line = lines.Dequeue();

        nameText.text = line.characterName;
        portraitImage.sprite = line.portrait;

        currentSentence = line.sentence;

        StartTyping(currentSentence);
    }

    private void StartTyping(string sentence)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeCoroutine(sentence));
    }

    private IEnumerator TypeCoroutine(string sentence)
    {
        IsTyping = true;

        foreach (char c in sentence)
        {
            dialogueText.text += c;
            _tipingSource.Play();
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialogueText.text = currentSentence;
        typingCoroutine = null;
        IsTyping = false;
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueBox.SetActive(false);
    }

}
