using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class ButtonMenu : MonoBehaviour
{
    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        AudioManager.instance.Play("Click");
    }

    public void RestartLevel()
    {
        AudioManager.instance.Play("Click");
        // Reset semua data quiz sebelum reload scene
        QuizManager quizManager = FindObjectOfType<QuizManager>();
        if (quizManager != null)
        {
            quizManager.ResetAllQuizData();
        }

        // Reload scene aktif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void BackMainMenu()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.Play("Click");
    }

    public void LoadScene(string sceneName)
    {
        if (SceneTransition.Instance != null)
        {
            SceneTransition.Instance.TransitionToScene(sceneName);
            AudioManager.instance.Play("Click");
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
        }
    }
}
