using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
    {
        PauseEscape();
    }

    private void PauseEscape()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            GameState newGameState = currentGameState == GameState.Gameplay
                ? GameState.Paused
                : GameState.Gameplay;

            GameStateManager.Instance.SetState(newGameState);
        }
    }
}
