using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public Transform destination;    
    public GameObject player;
    private bool hasTeleported = false; // Flag untuk memastikan teleport hanya sekali saat masuk

    private void Awake()
    {
        // player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasTeleported)
        {
            // Jika jarak antara player dan portal melebihi ambang (contoh 0.3f)
            if (Vector2.Distance(player.transform.position, transform.position) > 0.3f)
            {
                player.transform.position = destination.position;
                hasTeleported = true; // Set flag agar tidak terjadi teleport ulang
            }
        }
    }

    // Reset flag teleport ketika player keluar dari trigger
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && hasTeleported)
        {
            hasTeleported = false;
        }
    }
}
