using UnityEngine;
using UnityEngine.UI;

public class UIPanel : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject pauseMenuPanel;   // Panel Pause Menu (jika diperlukan)
    [SerializeField] private GameObject nextLevelPanel;     // Panel untuk "You Win" dan bintang

    [Header("Next Level Panel Elements")]
    [SerializeField] private GameObject[] starIcons;        // Array ikon bintang (misalnya 3 ikon)
    [SerializeField] private Text winText;                  // Text untuk menampilkan pesan "You Win"

    private void Start()
    {
        // Pastikan panel Next Level tidak tampil saat game mulai
        if (nextLevelPanel != null)
            nextLevelPanel.SetActive(false);
    }

    /// <summary>
    /// Menampilkan panel "You Win" beserta bintang yang diperoleh.
    /// </summary>
    /// <param name="starsEarned">Jumlah bintang (maksimal 3)</param>
    public void ShowNextLevelPanel(int starsEarned)
    {
        if (nextLevelPanel != null)
        {
            nextLevelPanel.SetActive(true);

            // Jika ada komponen winText, tampilkan pesan "You Win"
            if (winText != null)
                winText.text = "You Win!";

            // Aktifkan atau nonaktifkan ikon bintang berdasarkan starsEarned
            if (starIcons != null && starIcons.Length > 0)
            {
                for (int i = 0; i < starIcons.Length; i++)
                {
                    starIcons[i].SetActive(i < starsEarned);
                }
            }
        }
        else
        {
            Debug.LogWarning("NextLevelPanel belum diassign di UIPanel.");
        }
    }

    /// <summary>
    /// Menyembunyikan panel Next Level.
    /// </summary>
    public void HideNextLevelPanel()
    {
        if (nextLevelPanel != null)
        {
            nextLevelPanel.SetActive(false);
        }
    }
}
