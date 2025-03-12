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
}