using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class NextLevelController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private QuizManager quizManager; // Manager untuk quiz (opsional)
    [SerializeField] private GameObject interactPrompt; // UI prompt "Press E to interact"
    [SerializeField] private UIPanel uiPanel;           // Script UIPanel untuk menampilkan panel "You Win" beserta bintang

    [Header("Settings")]
    [SerializeField] private float quizTimeout = 15f; // Batas waktu quiz (dalam detik)
    [SerializeField] private bool isQuiz = true;        // Jika false, level tidak memerlukan quiz

    [Header("UI Timer")]
    [SerializeField] private Text quizTimerText;        // Text canvas untuk menampilkan countdown quiz

    private bool playerInRange = false;
    private bool isProcessing = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            if (interactPrompt != null)
                interactPrompt.SetActive(true);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isProcessing)
        {
            isProcessing = true; // Mencegah pemanggilan ganda

            if (isQuiz && quizManager != null)
            {
                quizManager.ShowQuiz();
                // Mulai coroutine untuk menunggu quiz atau timeout
                StartCoroutine(WaitForQuizOrTimeout(quizTimeout));
            }
            else
            {
                ProceedToNextLevel();
            }
        }
    }

    private IEnumerator WaitForQuizOrTimeout(float timeout)
    {
        float timer = timeout;

        // Aktifkan text timer dan tampilkan pesan "QuizTime!!" selama 2 detik
        if (quizTimerText != null)
        {
            quizTimerText.gameObject.SetActive(true);
            quizTimerText.text = "QuizTime!!";
        }
        yield return new WaitForSeconds(2f);

        // Sekarang mulai menampilkan countdown dengan format "QuizTimeout: {sisa waktu}"
        while (quizManager.IsQuizActive() && timer > 0f)
        {
            timer -= Time.deltaTime;
            if (quizTimerText != null)
                quizTimerText.text = " " + Mathf.Ceil(timer).ToString();
            yield return null;
        }

        // Setelah loop, sembunyikan text timer
        if (quizTimerText != null)
            quizTimerText.gameObject.SetActive(false);

        // Jika quiz masih aktif (waktu habis), sembunyikan quiz
        if (quizManager.IsQuizActive())
        {
            Debug.Log("Quiz timed out.");
            quizManager.HideQuiz();
        }
        ProceedToNextLevel();
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
        if (isQuiz && quizManager != null && quizManager.IsQuizPassed())
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
            AudioManager.instance.Play("Win");
        }
        else
        {
            Debug.LogWarning("UIPanel belum diassign di NextLevelController.");
        }

        UnlockNewLevel();
    }

    void UnlockNewLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
