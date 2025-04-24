using UnityEngine;
using UnityEngine.UI;

public class SetVolume : MonoBehaviour
{
    public Slider volumeSlider; // Drag slider UI ke sini melalui inspector
    public float volumeStep = 0.1f; // Besarnya penambahan atau pengurangan volume

    void Start()
    {
        if (volumeSlider == null)
            volumeSlider = GetComponent<Slider>();

        volumeSlider.value = 1f;
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
    }

    void OnVolumeSliderChanged(float value)
    {
        Debug.Log("Volume changed to: " + value);
        AudioManager.instance.SetVolume(value);
    }

    // Dipanggil dari tombol "Tambah Volume"
    public void IncreaseVolume()
    {
        volumeSlider.value = Mathf.Clamp(volumeSlider.value + volumeStep, 0f, 1f);
    }

    // Dipanggil dari tombol "Kurangi Volume"
    public void DecreaseVolume()
    {
        volumeSlider.value = Mathf.Clamp(volumeSlider.value - volumeStep, 0f, 1f);
    }
}
