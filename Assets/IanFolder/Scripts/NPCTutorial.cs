using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Lance
{
    public class NPCTutorial : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public Text dialogueText;
        public DialogueLine[] dialogueLines;
        private int index;

        public float wordSpeed;
        private bool playerIsClose;
        private bool dialogueStarted;
        private bool dialogueFinished;

        private bool isTyping;
        private bool skipCurrentSentence;

        public Image overlayImage;
        public Collider2D dialogueCollider;

        [System.Serializable]
        public class DialogueLine
        {
            public string dialogue;
            public GameObject[] uiToShow;
            public GameObject[] uiToHide;
        }

        private void Start()
        {
            dialogueStarted = false;
            dialogueFinished = false;
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }
        }

        private void Update()
        {
            if (playerIsClose && !dialogueStarted && !dialogueFinished)
            {
                if (!Input.GetButtonDown("InventoryBag")) // Check if the inventory button is not pressed
                {
                    StartDialogue();
                }
            }

            if (dialogueStarted && (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began || Input.GetMouseButtonDown(0)))
            {
                if (isTyping)
                {
                    skipCurrentSentence = true;
                }
                else
                {
                    NextDialogue();
                }
            }
        }

        private void StartDialogue()
        {
            Time.timeScale = 0;
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(true);
            }
            dialoguePanel.SetActive(true);
            dialogueStarted = true;
            StartCoroutine(Typing());
            if (dialogueCollider != null)
            {
                dialogueCollider.enabled = false;
            }

            // Show UI elements specified for the current dialogue line
            ShowUIForCurrentDialogue();
        }

        private void ShowUIForCurrentDialogue()
        {
            if (index < dialogueLines.Length)
            {
                foreach (GameObject uiElement in dialogueLines[index].uiToShow)
                {
                    uiElement.SetActive(true);
                }
                // Hide UI elements not specified for the current dialogue line
                foreach (GameObject uiElement in dialogueLines[index].uiToHide)
                {
                    uiElement.SetActive(false);
                }
            }
        }

        private IEnumerator Typing()
        {
            if (isTyping) yield break;

            isTyping = true;
            dialogueText.text = "";

            string currentDialogue = dialogueLines[index].dialogue;
            int dialogueLength = currentDialogue.Length;

            for (int i = 0; i < dialogueLength; i++)
            {
                if (skipCurrentSentence)
                {
                    dialogueText.text = currentDialogue;
                    break;
                }

                dialogueText.text += currentDialogue[i];
                yield return new WaitForSecondsRealtime(wordSpeed);
            }

            isTyping = false;
            skipCurrentSentence = false;

            if (index == dialogueLines.Length - 1 && !skipCurrentSentence) // Check if it's the last dialogue line and typing is not skipped
            {
                dialogueFinished = true;
            }
        }

        private void NextDialogue()
        {
            if (index < dialogueLines.Length - 1)
            {
                index++;
                StartCoroutine(Typing());
                ShowUIForCurrentDialogue();
            }
            else
            {
                EndDialogue();
            }
        }

        private void EndDialogue()
        {
            Time.timeScale = 1;
            dialoguePanel.SetActive(false);
            dialogueStarted = false;
            StopCoroutine(Typing()); // Ensure the typing coroutine is stopped when ending the dialogue
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }

            // Show all UI elements that were hidden during dialogue
            ShowAllUI();
        }

        private void ShowAllUI()
        {
            foreach (DialogueLine dialogueLine in dialogueLines)
            {
                foreach (GameObject uiElement in dialogueLine.uiToHide)
                {
                    uiElement.SetActive(true);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = true;
                dialogueText.text = "";
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                playerIsClose = false;
            }
        }
    }
}