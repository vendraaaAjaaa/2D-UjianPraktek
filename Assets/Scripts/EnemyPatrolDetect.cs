using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolDetect : Enemy
{
    [Header("Detection Settings")]
    [SerializeField] private float detectionRadius = 5f; // Radius deteksi untuk mendeteksi player
    private Transform player;

    protected override void Start()
    {
        base.Start();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    // Karena method Update() di Enemy bersifat private, kita cukup mendefinisikan method Update() di sini tanpa keyword 'new'.
    void Update()
    {
        // Pastikan player tersedia
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= detectionRadius)
            {
                // Jika player berada dalam radius, aktifkan animasi chase dan panggil fungsi patrol.
                enemyAnimator.SetBool("IsChasing", true);
                Patrol();
            }
            else
            {
                // Jika player di luar radius, nonaktifkan flag chase dan trigger animasi Hide.
                enemyAnimator.SetBool("IsChasing", false);
                enemyAnimator.SetTrigger("Hide");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Set warna Gizmo misalnya kuning
        Gizmos.color = Color.yellow;
        // Gambar sphere dengan center di transform.position dan radius detectionRadius
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
