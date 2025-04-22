using UnityEngine;
using UnityEngine.UIElements;
using Yarn.Unity;

public class NPCDialogueTrigger : MonoBehaviour
{
    public DialogueRunner dialogueRunner;
    public string startingNode = "Tutorial";
    public KeyCode interactionKey = KeyCode.E;

    private bool playerInRange = false;

    private void Start()
    {
        // Find the DialogueRunner in the scene
        dialogueRunner = FindObjectOfType<DialogueRunner>();

        if (dialogueRunner == null)
        {
            Debug.LogError("DialogueRunner not found! Ensure there's a Dialogue Manager in the scene.");
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        
            playerInRange = true;
            Debug.Log("Player entered NPC interaction zone.");
    }

    private void OnCollisionExit2D(Collision2D other)
    {
            playerInRange = false;
            Debug.Log("Player left NPC interaction zone.");
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactionKey))
        {

            if (dialogueRunner != null && !dialogueRunner.IsDialogueRunning)
            {
                dialogueRunner.StartDialogue(startingNode);
                Debug.Log("Dialogue started.");
            }
        }
    }
}
