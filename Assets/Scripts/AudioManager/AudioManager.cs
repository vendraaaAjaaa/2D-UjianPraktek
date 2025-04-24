using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioMixerGroup mixerGroup;
    public AudioMixer audioMixer;
    public Sound[] sounds;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return; // Tambahkan return agar kode di bawah tidak dijalankan setelah penghancuran
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = mixerGroup;
        }

        Play("BGM"); // Pindahkan ke sini setelah instance diinisialisasi
    }

    public void Play(string sound)
    {
        if (instance == null) // Pastikan instance tidak null
        {
            Debug.LogWarning("AudioManager instance is null!");
            return;
        }

        Sound s = Array.Find(instance.sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + sound + " not found!");
            return;
        }

        s.source.volume = s.volume * (1f + UnityEngine.Random.Range(-s.volumeVariance / 2f, s.volumeVariance / 2f));
        s.source.pitch = s.pitch * (1f + UnityEngine.Random.Range(-s.pitchVariance / 2f, s.pitchVariance / 2f));

        s.source.Play();
    }

    public void Stop(string sound)
    {
        if (instance == null) return;
        Sound s = Array.Find(instance.sounds, item => item.name == sound);
        if (s == null)
        {
            Debug.LogWarning($"Sound {sound} not found!");
            return;
        }
        s.source.Stop();
    }

    public void SetVolume(float volume)
    {
        if (volume <= 0.0001f)
            volume = 0.0001f;
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(volume) * 20);
        Debug.Log("Volume set to: " + volume);
    }
}
