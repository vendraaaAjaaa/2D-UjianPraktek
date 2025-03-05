using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Button : MonoBehaviour
{
    public void ResumeGame()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
    }

    public void RestartLevel()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackMainMenu()
    {
        GameStateManager.Instance.SetState(GameState.Gameplay);
        SceneManager.LoadScene("MainMenu");
    }
}
