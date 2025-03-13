using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Data")]
    [SerializeField] private QuizData[] quizDatas; // Array QuizData untuk mengatur lebih dari satu quiz
    private int currentQuizIndex = 0;

    [Header("UI References")]
    [SerializeField] private GameObject quizPanel;    // Panel UI quiz (pastikan nonaktif saat start)
    [SerializeField] private Text questionText;       // Komponen Text untuk pertanyaan
    [SerializeField] private Button[] optionButtons;    // Array Button untuk pilihan jawaban

    private bool quizPassed = false;

    private void Start()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    /// <summary>
    /// Menampilkan quiz berdasarkan indeks currentQuizIndex.
    /// Jika semua quiz telah ditampilkan, panel akan disembunyikan.
    /// </summary>
    public void ShowQuiz()
    {
        if (quizPanel == null)
        {
            Debug.LogError("QuizPanel belum diassign.");
            return;
        }

        if (quizDatas == null || quizDatas.Length == 0)
        {
            Debug.LogError("QuizDatas belum diassign atau kosong.");
            return;
        }

        // Jika indeks melebihi jumlah quiz, berarti tidak ada lagi quiz yang ditampilkan.
        if (currentQuizIndex >= quizDatas.Length)
        {
            Debug.Log("Semua quiz sudah selesai.");
            quizPanel.SetActive(false);
            return;
        }

        QuizData currentQuiz = quizDatas[currentQuizIndex];
        quizPanel.SetActive(true);
        questionText.text = currentQuiz.question;

        // Atur tampilan button untuk opsi jawaban
        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < currentQuiz.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = currentQuiz.options[i];
                int index = i; // Capture index untuk lambda
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Fungsi yang dipanggil saat sebuah opsi jawaban dipilih.
    /// Menentukan apakah jawaban benar dan menyembunyikan panel quiz.
    /// </summary>
    /// <param name="selectedIndex">Indeks jawaban yang dipilih</param>
    private void OnOptionSelected(int selectedIndex)
    {
        QuizData currentQuiz = quizDatas[currentQuizIndex];
        if (selectedIndex == currentQuiz.correctAnswerIndex)
        {
            quizPassed = true;
            Debug.Log("Quiz passed!");
        }
        else
        {
            quizPassed = false;
            Debug.Log("Quiz failed!");
        }

        // Sembunyikan panel quiz setelah memilih jawaban
        quizPanel.SetActive(false);

        // Jika Anda ingin menggunakan lebih dari satu quiz secara berurutan,
        // Anda bisa menambahkan logika untuk menaikkan indeks quiz.
        // Contoh:
        // currentQuizIndex++;
    }

    /// <summary>
    /// Mengembalikan status apakah quiz (yang terakhir ditampilkan) dijawab dengan benar.
    /// </summary>
    /// <returns>True jika jawaban benar; false jika salah.</returns>
    public bool IsQuizPassed()
    {
        return quizPassed;
    }

    /// <summary>
    /// (Opsional) Reset QuizManager untuk level berikutnya.
    /// </summary>
    public void ResetQuiz()
    {
        currentQuizIndex = 0;
        quizPassed = false;
        if (quizPanel != null)
            quizPanel.SetActive(false);
    }
}
