using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    [Header("Data Dialog")]
    public List<DialogueManager.DialogData> dialogs;

    [Header("Pengaturan Trigger")]
    public DialogueManager dialogueManager;
    public bool playOnce = true;

    [Header("UI Interaksi")]
    public GameObject interactPopup;

    private bool hasPlayed = false;
    private bool isPlayerInRange = false;

    private void Update()
    {
        if (isPlayerInRange && !hasPlayed && Input.GetKeyDown(KeyCode.E))
        {
            dialogueManager.StartDialogue(dialogs);
            if (playOnce) hasPlayed = true;

            if (interactPopup != null)
                interactPopup.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;

            if (interactPopup != null)
                interactPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;

            if (interactPopup != null)
                interactPopup.SetActive(false);
        }
    }
}
