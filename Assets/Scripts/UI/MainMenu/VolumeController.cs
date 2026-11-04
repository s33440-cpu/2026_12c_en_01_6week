using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer audioMixer;

    public Slider musicSlider;
    public Slider sfxSlider;

    void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume",
            Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume",
            Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f);
    }
}
