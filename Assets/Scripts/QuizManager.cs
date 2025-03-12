using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    [Header("Quiz Data")]
    [SerializeField] private QuizData quizData; // Data quiz yang diatur di Inspector

    [Header("UI References")]
    [SerializeField] private GameObject quizPanel; // Panel UI quiz (pastikan nonaktif saat start)
    [SerializeField] private Text questionText;
    [SerializeField] private Button[] optionButtons; // Array tombol untuk pilihan jawaban

    private bool quizPassed = false;

    private void Start()
    {
        if (quizPanel != null)
            quizPanel.SetActive(false);
    }

    public void ShowQuiz()
    {
        if (quizPanel == null || quizData == null)
        {
            Debug.LogError("QuizPanel atau QuizData belum diassign.");
            return;
        }
        quizPanel.SetActive(true);
        questionText.text = quizData.question;

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < quizData.options.Length)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].GetComponentInChildren<Text>().text = quizData.options[i];
                int index = i; // capture indeks untuk lambda
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => OnOptionSelected(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnOptionSelected(int selectedIndex)
    {
        if (selectedIndex == quizData.correctAnswerIndex)
        {
            quizPassed = true;
            Debug.Log("Quiz passed!");
        }
        else
        {
            quizPassed = false;
            Debug.Log("Quiz failed!");
        }
        // Sembunyikan quiz setelah memilih jawaban
        quizPanel.SetActive(false);
    }

    public bool IsQuizPassed()
    {
        return quizPassed;
    }
}
