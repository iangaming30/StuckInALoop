using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Lance
{
    public class NPC : MonoBehaviour
    {
        public GameObject dialoguePanel;
        public Text dialogueText;
        public string[] dialogue;
        private int index;

        public GameObject contButton;
        public GameObject backButton; 
        public float wordSpeed;
        private bool playerIsClose;
        private bool dialogueStarted;

        private bool isTyping;

        
        public Image overlayImage;

        
        public Collider2D dialogueCollider;

        
        private bool colliderTriggered;

        void Start()
        {
            dialogueStarted = false;
            colliderTriggered = false; 
            
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }

            
            contButton.SetActive(false);
            backButton.SetActive(false);
        }

        void Update()
        {
            
            if (playerIsClose && !dialogueStarted && !colliderTriggered)
            {
                StartDialogue();
            }

            
            if (!playerIsClose && dialogueStarted)
            {
                EndDialogue();
            }
        }

        void StartDialogue()
        {
            
            Time.timeScale = 0;

            
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(true);
            }

            dialoguePanel.SetActive(true);
            
            contButton.SetActive(true);
            StartCoroutine(Typing());
            dialogueStarted = true;
            colliderTriggered = true; 
                                      
            if (dialogueCollider != null)
            {
                dialogueCollider.enabled = false;
            }
        }


        IEnumerator Typing()
        {
            if (isTyping) yield break; 

            isTyping = true;
            dialogueText.text = ""; 

            string currentDialogue = dialogue[index]; 
            int dialogueLength = currentDialogue.Length;

            for (int i = 0; i < dialogueLength; i++)
            {
                dialogueText.text += currentDialogue[i];
                yield return new WaitForSecondsRealtime(wordSpeed); 
            }

            isTyping = false;

            
            contButton.SetActive(true);
            
            backButton.SetActive(index > 0);
            Debug.Log("Typing complete. Index: " + index + ". Back button active: " + (index > 0));
        }

        public void NextLine()
        {
            if (isTyping)
            {
                StopAllCoroutines();
                dialogueText.text = dialogue[index]; 
                isTyping = false;
                contButton.SetActive(true); 
                backButton.SetActive(index > 0); 
            }
            else
            {
                if (index < dialogue.Length - 1)
                {
                    index++;
                    dialogueText.text = "";
                    StartCoroutine(Typing());
                }
                else
                {
                    
                    EndDialogue();
                }
            }
        }

        public void PreviousLine()
        {
            if (isTyping)
            {
                StopAllCoroutines();
                isTyping = false;
            }

            if (index > 0)
            {
                index--; 
                dialogueText.text = dialogue[index]; 
                Debug.Log("Moved to previous line. Index: " + index);
            }

            
            backButton.SetActive(index > 0);
        }

        void EndDialogue()
        {
            
            Time.timeScale = 1;

            dialogueText.text = "";
            index = 0;
            dialoguePanel.SetActive(false); 
            contButton.SetActive(false); 
            backButton.SetActive(false); 
            dialogueStarted = false;

            
            if (overlayImage != null)
            {
                overlayImage.gameObject.SetActive(false);
            }
            Debug.Log("Dialogue ended.");
        }

        public void ContinueButtonClick()
        {
            NextLine();
        }

        public void BackButtonClick()
        {
            PreviousLine();
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