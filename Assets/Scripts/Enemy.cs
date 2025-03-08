using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Data")]
    public EnemyData enemyData;  // Data enemy yang diassign dari asset EnemyData

    [Header("Patrol Settings")]
    public Transform pointA;     // Titik patroli pertama
    public Transform pointB;     // Titik patroli kedua

    [Header("Components")]
    public Animator enemyAnimator;

    private int currentHealth;
    private Transform targetPoint;    // Titik yang sedang dituju
    private bool facingRight = true;  // Untuk mengatur arah hadap enemy

    private void Start()
    {
        if (enemyData != null)
        {
            currentHealth = enemyData.maxHealth;
        }
        else
        {
            Debug.LogError("EnemyData belum diassign di " + gameObject.name);
        }

        // Mulai patrolling, misalnya menuju pointB terlebih dahulu
        targetPoint = pointB;
    }

    private void Update()
    {
        Patrol();
    }

    // Fungsi patrolling antara pointA dan pointB
    private void Patrol()
    {
        if (pointA == null || pointB == null)
            return;

        // Pindahkan enemy menuju targetPoint menggunakan kecepatan dari enemyData.moveSpeed
        transform.position = Vector2.MoveTowards(transform.position, targetPoint.position, enemyData.moveSpeed * Time.deltaTime);

        // Jika sudah mendekati targetPoint, ganti target dan balik arah
        if (Vector2.Distance(transform.position, targetPoint.position) < 0.1f)
        {
            targetPoint = (targetPoint == pointB) ? pointA : pointB;
            Flip();
        }
    }

    // Fungsi untuk membalik arah enemy
    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    // Fungsi untuk menerima damage (misalnya dari serangan player)
    public void TakeDamage(int damageAmount)
    {
        int effectiveDamage = Mathf.Max(damageAmount - enemyData.defense, 1);

        currentHealth -= effectiveDamage;
        Debug.Log(enemyData.enemyName + " menerima damage: " + effectiveDamage);

        PlayHitAnimation();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void PlayHitAnimation()
    {
        if (enemyAnimator != null)
        {
            enemyAnimator.SetTrigger("Hit");
        }
        else
        {
            Debug.LogWarning("Animator tidak diassign pada enemy " + gameObject.name);
        }
    }

    // Fungsi yang dipanggil saat enemy mati
    private void Die()
    {
        Debug.Log(enemyData.enemyName + " telah mati.");
        // Tambahkan animasi atau efek mati jika diperlukan
        Destroy(gameObject);
    }

    // Fungsi ini akan dipanggil saat enemy bersentuhan dengan player untuk menyerang
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Pastikan player memiliki tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Misalnya, player memiliki script PlayerHealth yang mengatur health
            HealthBar playerHealth = collision.gameObject.GetComponent<HealthBar>();
            if (playerHealth != null)
            {
                // Gunakan nilai damage dari enemyData, sehingga tiap enemy bisa memiliki damage berbeda
                playerHealth.TakeDamage(enemyData.damage);
            }
        }
    }
}
