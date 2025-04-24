using System.Collections;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 lastCheckpoint;
    private bool isDead = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr; // Biar bisa disable sprite kalau mau
    // public Animator animator;

    void Start()
    {
        lastCheckpoint = transform.position;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetCheckpoint(Vector3 newCheckpoint)
    {
        lastCheckpoint = newCheckpoint;
    }

   public void Respawn()
    {
        // animator.SetBool("isDead", false);
        transform.position = lastCheckpoint;
        Debug.Log("Player Respawned at: " + lastCheckpoint);
        isDead = false;
        
    }
}