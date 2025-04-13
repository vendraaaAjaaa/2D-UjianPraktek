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

    protected int currentHealth;
    private Transform targetPoint;    // Titik yang sedang dituju
    private bool facingRight = true;    // Untuk mengatur arah hadap enemy

    protected virtual void Start()
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

    protected void Patrol()
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

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

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

    public void Die()
    {
        Debug.Log(enemyData.enemyName + " telah mati.");
        Destroy(gameObject);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HealthBar playerHealth = collision.gameObject.GetComponent<HealthBar>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(enemyData.damage);
            }
        }
    }
}
