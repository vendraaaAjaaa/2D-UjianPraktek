using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DeadZone : MonoBehaviour
{
    [Header("Player Death Settings")]
    [Tooltip("Optional tag filter for the player")] public string playerTag = "Player";
    public GameOverManager gameOverManager;

    private void Awake()
    {
        // Ensure this collider is a trigger
        Collider2D col = GetComponent<Collider2D>();
        if (!col.isTrigger)
            col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag(playerTag))
            return;

        // Get the PlayerRespawn component
        PlayerRespawn pr = other.GetComponent<PlayerRespawn>();
        pr.Respawn();
        gameOverManager.ShowGameOver();
    }
}
