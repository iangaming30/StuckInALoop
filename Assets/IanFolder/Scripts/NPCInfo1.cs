using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lance
{
    public class NPCInfo1 : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public Text dialogueText;
        public string[] dialogue;
        private int index;

        public float wordSpeed;
        private bool playerIsClose;
        private bool dialogueStarted;
        private bool dialogueFinished;

        private bool isTyping;
        private bool skipCurrentSentence;

        public Image overlayImage;
        public Collider2D dialogueCollider;

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
                StartDialogue();
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
        }

        private IEnumerator Typing()
        {
            if (isTyping) yield break;

            isTyping = true;
            dialogueText.text = "";

            string currentDialogue = dialogue[index];
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

            if (index == dialogue.Length - 1 && !skipCurrentSentence) // Check if it's the last dialogue line and typing is not skipped
            {
                dialogueFinished = true;
            }
        }

        private void NextDialogue()
        {
            if (index < dialogue.Length - 1)
            {
                index++;
                StartCoroutine(Typing());
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