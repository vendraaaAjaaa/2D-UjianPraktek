using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverPanel; // Panel UI Game Over

    private void Start()
    {
        // Pastikan panel Game Over tidak tampil saat game mulai
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    // Fungsi untuk menampilkan UI Game Over dan pause game
    public void ShowGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            GameStateManager.Instance.SetState(GameState.Paused);
        }
        else
        {
            Debug.LogWarning("GameOverPanel belum diassign di GameOverManager.");
        }

        // // Panggil PauseManager (yang sudah menggunakan Unity Event) untuk pause game
        // if (PauseManager.Instance != null)
        // {
        //     PauseManager.Instance.PauseGame();
        // }
        // else
        // {
        //     Debug.LogWarning("PauseManager instance tidak ditemukan.");
        // }
    }
}