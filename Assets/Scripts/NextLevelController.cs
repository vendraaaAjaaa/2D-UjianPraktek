using UnityEngine;
using UnityEngine.SceneManagement;

public class NextLevelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuizManager quizManager; // Manager untuk quiz (opsional)
    [SerializeField] private GameObject interactPrompt; // UI prompt "Press E to interact"
    [SerializeField] private UIPanel uiPanel;           // Script UIPanel untuk menampilkan panel "You Win" beserta bintang

    [Header("Settings")]
    [SerializeField] private float interactKeyDelay = 0.5f; // Delay agar quiz dapat diproses sebelum lanjut

    private bool playerInRange = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = false;
            if (interactPrompt != null)
                interactPrompt.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            // Jika ada QuizManager, tampilkan quiz terlebih dahulu
            if (quizManager != null)
            {
                quizManager.ShowQuiz();
                // Setelah delay, lanjutkan perhitungan bintang dan tampilkan UI You Win
                Invoke("ProceedToNextLevel", interactKeyDelay);
            }
            else
            {
                ProceedToNextLevel();
            }
        }
    }

    private void ProceedToNextLevel()
    {
        int stars = 0;

        // Kondisi 1: Jika semua musuh sudah mati, dapat 1 bintang
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0)
        {
            stars += 1;
        }

        // Kondisi 2: Jika quiz dijawab dengan benar, dapat 1 bintang
        if (quizManager != null && quizManager.IsQuizPassed())
        {
            stars += 1;
        }

        // Kondisi 3: Interaksi finish trigger selalu memberikan 1 bintang
        stars += 1;

        Debug.Log("Stars Awarded: " + stars);

        // Tampilkan UI "You Win" (Next Level Panel) yang menampilkan bintang yang diperoleh
        if (uiPanel != null)
        {
            uiPanel.ShowNextLevelPanel(stars);
        }
        else
        {
            Debug.LogWarning("UIPanel belum diassign di NextLevelController.");
        }
    }
}
