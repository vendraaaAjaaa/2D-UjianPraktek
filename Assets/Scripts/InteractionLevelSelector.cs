using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class InteractionLevelSelector : MonoBehaviour
{
    [Header("Interaction Settings")]
    public KeyCode interactKey = KeyCode.E;
    public GameObject interactPrompt;    // UI prompt to press key

    [Header("Level Canvas")]
    public GameObject levelCanvas;       // Canvas panel with level buttons

    private bool playerInRange = false;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.SetActive(false);
        if (levelCanvas != null)
            levelCanvas.SetActive(false);
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleLevelCanvas();
            AudioManager.instance.Play("Click");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
            // Hide level canvas if player walks away
            if (levelCanvas != null)
                levelCanvas.SetActive(false);
        }
    }

    private void ToggleLevelCanvas()
    {
        if (levelCanvas != null)
        {
            bool isActive = levelCanvas.activeSelf;
            levelCanvas.SetActive(!isActive);
            GameStateManager.Instance.SetState(GameState.Paused);
        }
    }
}
