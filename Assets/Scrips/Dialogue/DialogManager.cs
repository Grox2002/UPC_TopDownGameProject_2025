using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogManager : MonoBehaviour
{
    public static DialogManager Instance { get; private set; }

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
    private float dialogueCooldown = 0.2f;
    private float cooldownTimer = 0f;

    public bool IsDialogueActive { get; private set; }
    public bool IsTyping { get; private set; }

    public bool ignoreNextInput;

    //===========================================Metodos===================================================
    private void Awake() 
    {
        Instance = this;
    }

    private void Start()
    {
        dialogueBox.SetActive(false);
    }

    private void Update()
    {
        if (cooldownTimer > 0)
            cooldownTimer -= Time.deltaTime;

        if (!IsDialogueActive) return;

        if (ignoreNextInput)
        {
            ignoreNextInput = false;
            return; // ignoramos la tecla que inició el diálogo
        }

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

    public void OnInteractKeyPressed()
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

    public void StartDialogue(DialogSO dialogue)
    {
        if (dialogue == null) return;

        if (cooldownTimer > 0) return;

        IsDialogueActive = true;


        StartCoroutine(StartDialogueDelayed(dialogue));
    }

    private IEnumerator StartDialogueDelayed(DialogSO dialogue)
    {
        yield return null; 
                           
        dialogueBox.SetActive(true);
        dialogueText.text = "";
        lines.Clear();

        foreach (var line in dialogue.dialogueLines)
            lines.Enqueue(line);

        ignoreNextInput = true;

        DisplayNextLine();

    }

    private void DisplayNextLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        } 

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

        foreach (char character in sentence)
        {
            dialogueText.text += character;
            if(IsDialogueActive)
            _tipingSource.Play();
            yield return new WaitForSeconds(typingSpeed);
        }

        IsTyping = false;
        typingCoroutine = null;
    }

    private void SkipTyping()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
            

        dialogueText.text = currentSentence;
        IsTyping = false;
    }

    public void EndDialogue()
    {
        IsDialogueActive = false;
        dialogueBox.SetActive(false);
        cooldownTimer = dialogueCooldown;
    }

    public IEnumerator WaitForDialogue()
    {
        
        yield return new WaitUntil(() => IsDialogueActive);

        
        yield return new WaitUntil(() => !IsDialogueActive);
    }

}
