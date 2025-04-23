using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

[RequireComponent(typeof(Collider2D))]
public class BossEnemy : MonoBehaviour
{
    [Header("Boss Data (ScriptableObject)")]
    public EnemyData bossData;

    [Header("Player Reference")]
    public Transform player;

    [Header("Battle Zone Collider")]
    private Collider2D battleZone;
    private Vector3 lastInsidePosition;

    [Header("Cinemachine Cameras")]
    public CinemachineVirtualCamera normalCam;   // Virtual Camera for normal gameplay
    public CinemachineVirtualCamera battleCam;   // Virtual Camera for boss arena (with Confiner)

    [Header("Attack Settings")]
    public float meleeRange = 2f;
    public float meleeCooldown = 2f;
    public float rangedInterval = 30f;

    [Header("Projectile Settings")]
    public GameObject projectilePrefab;
    public Transform projectileSpawnPoint;
    public float projectileSpeed = 10f;

    [Header("Animator")]
    public Animator animator;
    private static readonly int RangedAttackTrigger = Animator.StringToHash("RangedAttack");
    private static readonly int MeleeAttackTrigger = Animator.StringToHash("MeleeAttack");

    [Header("Health Bar UI")]
    public GameObject healthBarUI;
    public Slider healthSlider;

    private float currentHealth;
    private float meleeTimer;
    private float rangedTimer;
    private bool inBattle = false;

    private void Awake()
    {
        battleZone = GetComponent<Collider2D>();
        currentHealth = bossData ? bossData.maxHealth : 100;
        meleeTimer = meleeCooldown;
        rangedTimer = 0f;

        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        if (healthSlider != null)
        {
            healthSlider.maxValue = currentHealth;
            healthSlider.value = currentHealth;
        }

        // Ensure battleCam is disabled priority at start
        if (normalCam != null && battleCam != null)
        {
            normalCam.Priority = 10;
            battleCam.Priority = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!inBattle && other.CompareTag("Player"))
        {
            inBattle = true;

            // Show health UI
            if (healthBarUI != null)
                healthBarUI.SetActive(true);

            // Switch to battle cinemachine camera
            if (normalCam != null && battleCam != null)
            {
                battleCam.Priority = normalCam.Priority + 1;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (inBattle && other.CompareTag("Player"))
            lastInsidePosition = other.transform.position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (inBattle && other.CompareTag("Player"))
            other.transform.position = lastInsidePosition;
    }

    private void Update()
    {
        if (!inBattle || player == null || bossData == null)
            return;

        meleeTimer += Time.deltaTime;
        rangedTimer += Time.deltaTime;
        float distance = Vector2.Distance(transform.position, player.position);

        if (rangedTimer >= rangedInterval)
        {
            RangedAttack();
            rangedTimer = 0f;
        }
        else if (distance > meleeRange)
        {
            ChasePlayer();
        }
        else if (meleeTimer >= meleeCooldown)
        {
            MeleeAttack();
            meleeTimer = 0f;
        }
    }

    private void ChasePlayer()
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            bossData.moveSpeed * Time.deltaTime);
    }

    private void MeleeAttack()
    {
        if (animator != null)
            animator.SetTrigger(MeleeAttackTrigger);
        HealthBar hb = player.GetComponent<HealthBar>();
        if (hb != null)
            hb.TakeDamage(bossData.damage);
    }

    private void RangedAttack()
    {
        if (animator != null)
            animator.SetTrigger(RangedAttackTrigger);
        if (projectilePrefab != null && projectileSpawnPoint != null)
        {
            var proj = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
            var rb = proj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 dir = (player.position - projectileSpawnPoint.position).normalized;
                rb.velocity = dir * projectileSpeed;
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        int effectiveDamage = Mathf.Max(damageAmount - bossData.defense, 1);
        currentHealth -= effectiveDamage;
        Debug.Log(bossData.enemyName + " menerima damage: " + effectiveDamage);

        PlayHitAnimation();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void PlayHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("Hit");
        }
        else
        {
            Debug.LogWarning("Animator tidak diassign pada enemy " + gameObject.name);
        }
    }

    private void Die()
    {
        inBattle = false;
        if (healthBarUI != null)
            healthBarUI.SetActive(false);

        // Switch back to normal camera
        if (normalCam != null && battleCam != null)
        {
            battleCam.Priority = 0;
        }

        Destroy(gameObject);
    }
}
