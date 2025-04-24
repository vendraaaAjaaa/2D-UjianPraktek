using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quiz", menuName = "Quiz Data", order = 52)]
public class QuizData : ScriptableObject
{
    [Header("Quiz Settings")]
    public string question;
    public string[] options;       // Pilihan jawaban
    public int correctAnswerIndex; // Indeks jawaban yang benar (misalnya 0, 1, 2, dst.)
    
    [Header("Progress")]
    public bool isDone;            // Tandai quiz sebagai selesai jika true
    public bool isPassed;


    private void OnEnable()
    {
        // Saat asset di-load ke memory, ini akan di-trigger (namun tidak selalu terjadi pada restart scene)
        isDone = false;
        isPassed = false;
        // if (restartLevel == true)
        // {
        //     ResetData();
        // }

    }

    /// <summary>
    /// Method untuk mengatur ulang status quiz
    /// </summary>
    public void ResetData()
    {
        Debug.Log("Fungsi Reset Data di QuizData");
        isDone = false;
        isPassed = false;
    }
}