using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    // Komponen Image yang menampilkan health bar di Canvas
    public Image healthBarImage;

    // Array sprite animasi health bar, diurutkan dari penuh (index 0) ke habis (index terakhir)
    public Sprite[] healthSprites;

    // Nilai maksimal health
    public int maxHealth = 100;
    // Nilai health saat ini
    private int currentHealth;

    public GameOverManager gameOverManager;

    void Start()
    {
        // Inisialisasi health player
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    // Fungsi untuk mengurangi health (dipanggil saat player terkena hit)
    public void TakeDamage(int damage)
    {
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            if (gameOverManager != null)
                gameOverManager.ShowGameOver();
            else
                Debug.LogWarning("GameOverManager belum diassign pada HealthBar.");
        }
    }

    // Update tampilan health bar berdasarkan nilai health saat ini
    void UpdateHealthBar()
    {
        // Hitung persentase health (0 - 1)
        float healthPercentage = (float)currentHealth / maxHealth;

        // Misalnya, jika kamu punya 5 sprite (index 0: penuh, index 4: habis),
        // maka kita hitung index sprite berdasarkan selisih health.
        int spriteIndex = Mathf.Clamp(
            Mathf.FloorToInt((1 - healthPercentage) * (healthSprites.Length - 1)),
            0,
            healthSprites.Length - 1
        );

        // Ganti sprite health bar sesuai index yang sudah dihitung
        healthBarImage.sprite = healthSprites[spriteIndex];
    }
}
