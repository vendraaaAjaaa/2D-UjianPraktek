using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Simpan posisi checkpoint
            collision.GetComponent<PlayerRespawn>().SetCheckpoint(transform.position);
            Debug.Log("Checkpoint Set: " + transform.position);
        }
    }
}