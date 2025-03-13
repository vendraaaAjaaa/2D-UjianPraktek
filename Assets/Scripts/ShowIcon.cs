using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ShowIcon : MonoBehaviour
{
    [Header("Icon References")]
    [SerializeField] private GameObject controllerIcon; // Icon untuk controller (misalnya dengan label "X")
    [SerializeField] private GameObject keyboardIcon;   // Icon untuk keyboard (misalnya dengan label "J")
    
    // private bool playerInTrigger = false;

    private void Start()
    {
        // Pastikan kedua icon tidak muncul saat game dimulai
        if (controllerIcon != null) controllerIcon.SetActive(false);
        if (keyboardIcon != null) keyboardIcon.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Pastikan yang masuk adalah player
        if (collision.CompareTag("Player"))
        {
            // playerInTrigger = true;
            UpdateIconDisplay();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        // Saat player tetap berada di dalam trigger, periksa input device secara berkala
        if (collision.CompareTag("Player"))
        {
            UpdateIconDisplay();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Saat player keluar dari trigger, sembunyikan kedua icon
        if (collision.CompareTag("Player"))
        {
            // playerInTrigger = false;
            if (controllerIcon != null) controllerIcon.SetActive(false);
            if (keyboardIcon != null) keyboardIcon.SetActive(false);
        }
    }

    // Fungsi untuk memperbarui tampilan icon sesuai dengan perangkat input yang terdeteksi
    private void UpdateIconDisplay()
    {
        // Menggunakan sistem Input System baru: jika ada gamepad terdeteksi
        if (Gamepad.current != null)
        {
            if (controllerIcon != null) controllerIcon.SetActive(true);
            if (keyboardIcon != null) keyboardIcon.SetActive(false);
        }
        else
        {
            if (controllerIcon != null) controllerIcon.SetActive(false);
            if (keyboardIcon != null) keyboardIcon.SetActive(true);
        }
    }
}
 