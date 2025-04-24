using UnityEngine;

/// <summary>
/// Generic 2D projectile: moves in a direction, damages targets, and auto-destroys.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class Projectile2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("Movement speed of the projectile.")] public float speed = 10f;
    [Tooltip("Normalized direction vector for movement.")] public Vector2 direction = Vector2.right;

    [Header("Damage Settings")]
    [Tooltip("Damage dealt when hitting a target.")] public int damage = 10;
    [Tooltip("Layer mask specifying which layers this projectile can hit.")] public LayerMask hitLayers;

    [Header("Lifetime")]
    [Tooltip("Time in seconds before projectile self-destructs.")] public float lifetime = 5f;

    private void Start()
    {
        // Destroy after lifetime to prevent clutter
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        // Move each frame
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Only process hits on designated layers
        if ((hitLayers.value & (1 << other.gameObject.layer)) == 0)
            return;

        // Damage target if it has a HealthBar component
        var hb = other.GetComponent<HealthBar>();
        if (hb != null)
            hb.TakeDamage(damage);

        // Destroy the projectile on hit
        Destroy(gameObject);
    }

    /// <summary>
    /// Initialize projectile parameters at runtime.
    /// </summary>
    /// <param name="dir">Direction vector (normalized).</param>
    /// <param name="spd">Speed value.</param>
    /// <param name="dmg">Damage value.</param>
    public void Initialize(Vector2 dir, float spd, int dmg)
    {
        direction = dir.normalized;
        speed = spd;
        damage = dmg;
    }
}
