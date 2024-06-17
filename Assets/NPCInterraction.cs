using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionIcon; // The icon to display
    private bool isPlayerNear = false;

    void Start()
    {
        interactionIcon.SetActive(false); // Hide the icon at the start
    }

    void Update()
    {
        if (isPlayerNear && Input.GetKeyDown(KeyCode.E))
        {
            // Trigger the conversation or any other interaction here
            Debug.Log("Engage conversation with NPC");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionIcon.SetActive(true); // Show the icon
            isPlayerNear = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionIcon.SetActive(false); // Hide the icon
            isPlayerNear = false;
        }
    }
}
