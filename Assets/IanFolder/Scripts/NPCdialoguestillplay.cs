using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lance
{
    public class NPCdialoguestillplay : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public Text dialogueText;
        public string[] dialogue;
        private int index;

        public float wordSpeed;
        public float lineDelay; // Add a new variable for line delay
        private bool dialogueStarted;

        private bool isTyping;

        // Reference to the UI overlay image
        public Image overlayImage;

        // Flag to track if the collider has triggered the dialogue
        private bool dialogueTriggered;

        private Coroutine typingCoroutine; // Store reference to the typing coroutine

        private void Start()
        {
            dialogueStarted = false;
            dialogueTriggered = false; // Initialize the flag
            // Ensure the overlay is initially disabled
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }
        }

        // Call this method to start the dialogue
        public void TriggerDialogue()
        {
            if (!dialogueStarted && !dialogueTriggered)
            {
                StartDialogue();
            }
        }

        private void StartDialogue()
        {
            // Enable the overlay to darken the background
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(true);
            }

            dialoguePanel.SetActive(true);
            // Start the typing coroutine and store the reference
            typingCoroutine = StartCoroutine(Typing());
            dialogueStarted = true;
            dialogueTriggered = true; // Set the dialogue-triggered flag
        }

        private IEnumerator Typing()
        {
            isTyping = true;
            dialogueText.text = ""; // Clear the dialogue text before starting typing

            string currentDialogue = dialogue[index]; // Get the current dialogue line
            int dialogueLength = currentDialogue.Length;

            for (int i = 0; i < dialogueLength; i++)
            {
                dialogueText.text += currentDialogue[i];
                yield return new WaitForSeconds(wordSpeed);
            }

            isTyping = false;

            // Add a delay before displaying the next line
            yield return new WaitForSeconds(lineDelay);

            // Check if it's the last line of dialogue
            if (index == dialogue.Length - 1)
            {
                yield return new WaitForSeconds(1.5f); // Add a delay before hiding the dialogue panel
                EndDialogue();
            }
            else
            {
                index++;
                // Restart the typing coroutine with updated word speed
                typingCoroutine = StartCoroutine(Typing());
            }
        }

        private void EndDialogue()
        {
            dialogueText.text = "";
            index = 0;
            dialoguePanel.SetActive(false); // Disable the entire dialogue panel
            dialogueStarted = false;

            // Disable the overlay when ending the dialogue
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                TriggerDialogue();
            }
        }

        // Method to update word speed
        public void UpdateWordSpeed(float newWordSpeed)
        {
            wordSpeed = newWordSpeed;
            // If typing coroutine is running, stop and restart with new word speed
            if (isTyping && typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                typingCoroutine = StartCoroutine(Typing());
            }
        }
    }
}



