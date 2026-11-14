using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabDialogue : MonoBehaviour
{
    public Transform player;
    public GameObject dialoguePaper;
    public Transform lettersParent;
    public GameObject nextButton;

    public string[] dialogueLines =
    {
        "HEY WELCOME",
        "YOU NEED TO FINISH THIS LEVEL",
        "DONT WORRY I WILL HELP YOU",
        "PRESS A TO MOVE LEFT",
        "PRESS D TO MOVE RIGHT",
        "PRESS SPACE TO JUMP",
        "PRESS SPACE AGAIN TO DOUBLE JUMP"
    };

    public float interactionDistance = 2.5f;
    public float letterDelay = 0.06f;
    public float letterSpacing = 0.14f;
    public float lineSpacing = 0.25f;
    public int maxLettersPerLine = 18;
    public float letterScale = 0.85f;

    public Sprite[] letterSprites = new Sprite[26];
    public AudioClip typingSound;
    public AudioSource audioSource;
    public float typingVolume = 0.25f;
    public bool isFinalCrab = false;

    public string[] missingCoinsLines =
    {
        "HEY YOU MADE IT",
        "BUT YOU MISSED SOME COINS",
        "GO BACK AND FIND THEM"
    };

    public string[] allCoinsLines =
    {
        "WOW YOU DID IT",
        "YOU COLLECTED ALL COINS",
        "YOU CAN GO TO LEVEL TWO"
    };

    private bool dialogueActive;
    private bool dialogueFinished;
    private bool isTyping;
    private bool canEnterNextLevel;
    private bool playerLeftAfterDialogue = true;
    private int currentLine;
    private Coroutine typingCoroutine;
    private string[] activeLines;
    private CrabFinal crabFinal;
    private readonly List<GameObject> createdLetters = new List<GameObject>();
    private struct LetterInfo
    {
        public char character;
        public int column;
        public int row;

        public LetterInfo(char character, int column, int row)
        {
            this.character = character;
            this.column = column;
            this.row = row;
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        crabFinal = GetComponent<CrabFinal>();
        if (dialoguePaper != null)
            dialoguePaper.SetActive(false);

        if (nextButton != null)
            nextButton.SetActive(false);
    }

    private void Update()
    {
        if (player == null ||
            dialoguePaper == null ||
            lettersParent == null)
            return;
        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        if (distance > interactionDistance)
        {
            playerLeftAfterDialogue = true;
        }

        if (!dialogueActive &&
            (!dialogueFinished || isFinalCrab) &&
            (!isFinalCrab || playerLeftAfterDialogue) &&
            distance <= interactionDistance)
        {
            StartDialogue();
        }

        if (dialogueActive && distance > interactionDistance)
        {
            StopDialogue();
            return;
        }
        if (dialogueActive && Input.GetKeyDown(KeyCode.Q))
        {
            if (isTyping)
                return;
            currentLine++;

            if (currentLine >= activeLines.Length)
            {
                if (isFinalCrab &&
                    canEnterNextLevel &&
                    crabFinal != null)
                {
                    crabFinal.FinishLevel();
                    return;
                }

                dialogueFinished = true;
                StopDialogue();
            }
            else
            {
                ShowCurrentLine();
            }
        }
    }

    private void StartDialogue()
    {
        canEnterNextLevel = false;
        if (isFinalCrab)
        {
            if (crabFinal == null)
            {
                Debug.LogWarning(
                    "CrabFinal component is missing!"
                );
                return;
            }

            canEnterNextLevel = crabFinal.CanFinishLevel();

            if (canEnterNextLevel)
                activeLines = allCoinsLines;
            else
                activeLines = missingCoinsLines;
        }
        else
        {
            activeLines = dialogueLines;
        }

        if (activeLines == null || activeLines.Length == 0)
            return;

        if (isFinalCrab)
            playerLeftAfterDialogue = false;

        dialogueActive = true;
        currentLine = 0;

        dialoguePaper.SetActive(true);

        if (nextButton != null)
            nextButton.SetActive(false);

        ShowCurrentLine();
    }

    private void ShowCurrentLine()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        isTyping = true;

        if (nextButton != null)
            nextButton.SetActive(false);

        ClearLetters();

        typingCoroutine = StartCoroutine(
            TypeText(activeLines[currentLine])
        );
    }

    private IEnumerator TypeText(string text)
    {
        List<LetterInfo> letters = PrepareText(text);
        int letterCount = 0;
        foreach (LetterInfo letter in letters)
        {
            CreateLetter(letter);
            if (typingSound != null && audioSource != null)
            {
                if (letterCount % 2 == 0)
                {
                    audioSource.Stop();
                    audioSource.clip = typingSound;
                    audioSource.volume = typingVolume;
                    audioSource.Play();
                }
            }

            letterCount++;
            yield return new WaitForSeconds(letterDelay);
        }
        if (audioSource != null)
            audioSource.Stop();
        isTyping = false;
        typingCoroutine = null;
        if (nextButton != null)
            nextButton.SetActive(true);
    }

    private List<LetterInfo> PrepareText(string text)
    {
        List<LetterInfo> result = new List<LetterInfo>();
        int column = 0;
        int row = 0;
        string[] words = text.ToUpper().Split(' ');
        int lineLimit = Mathf.Max(1, maxLettersPerLine);
        foreach (string word in words)
        {
            if (column > 0 &&
                column + 1 + word.Length > lineLimit)
            {
                row++;
                column = 0;
            }
            else if (column > 0)
            {
                column++;
            }

            foreach (char character in word)
            {
                if (column >= lineLimit)
                {
                    row++;
                    column = 0;
                }

                result.Add(
                    new LetterInfo(character, column, row)
                );

                column++;
            }
        }
        return result;
    }

    private void CreateLetter(LetterInfo letter)
    {
        int index = letter.character - 'A';
        if (index < 0 || index >= letterSprites.Length)
            return;

        Sprite sprite = letterSprites[index];
        if (sprite == null)
            return;

        GameObject letterObject = new GameObject("Letter_" + letter.character);
        letterObject.transform.SetParent(lettersParent, false);
        letterObject.transform.localPosition = new Vector3(letter.column * letterSpacing, -letter.row * lineSpacing, 0);
        letterObject.transform.localScale =
            Vector3.one * letterScale;
        SpriteRenderer renderer =
            letterObject.AddComponent<SpriteRenderer>();

        renderer.sprite = sprite;
        renderer.color = Color.white;
        renderer.sortingLayerName = "Player";
        renderer.sortingOrder = 21;
        createdLetters.Add(letterObject);
    }

    private void ClearLetters()
    {
        foreach (GameObject letter in createdLetters)
        {
            if (letter != null)
            {
                letter.SetActive(false);
                Destroy(letter);
            }
        }

        createdLetters.Clear();
    }

    private void StopDialogue()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueActive = false;
        isTyping = false;
        if (nextButton != null)
            nextButton.SetActive(false);
        ClearLetters();
        if (dialoguePaper != null)
            dialoguePaper.SetActive(false);
    }
}