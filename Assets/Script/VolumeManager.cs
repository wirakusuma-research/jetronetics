using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI; // Tambahkan ini jika Anda ingin mengupdate slider di awal

public class VolumeManager : MonoBehaviour
{
    [Tooltip("Seret Audio Mixer di sini.")]
    public AudioMixer mainMixer;

    [Tooltip("Seret Slider Musik di sini untuk di-update saat game dimulai.")]
    public Slider musicSlider;
    [Tooltip("Seret Slider SFX di sini untuk di-update saat game dimulai.")]
    public Slider sfxSlider;

    // Nama kunci untuk menyimpan data di PlayerPrefs
    private const string MUSIC_VOLUME_KEY = "MusicVolume";
    private const string SFX_VOLUME_KEY = "SFXVolume";

    void Start()
    {
        // Muat volume yang sudah tersimpan saat game dimulai
        LoadVolumes();
    }

    /// <summary>
    /// Mengatur volume untuk musik. Dipanggil oleh slider UI.
    /// </summary>
    public void SetMusicVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        mainMixer.SetFloat("MusicVolume", mixerVolume);

        // Simpan nilai slider
        PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, volume);
    }

    /// <summary>
    /// Mengatur volume untuk sound effect. Dipanggil oleh slider UI.
    /// </summary>
    public void SetSFXVolume(float volume)
    {
        float mixerVolume = Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20f;
        mainMixer.SetFloat("SFXVolume", mixerVolume);

        // Simpan nilai slider
        PlayerPrefs.SetFloat(SFX_VOLUME_KEY, volume);
    }

    /// <summary>
    /// Memuat volume dari PlayerPrefs dan mengaturnya di mixer.
    /// </summary>
    private void LoadVolumes()
    {
        // Muat nilai yang tersimpan, atau gunakan nilai default 1.0 jika belum ada
        float musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 1.0f);
        float sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1.0f);

        // Atur slider ke nilai yang dimuat
        if (musicSlider != null)
        {
            musicSlider.value = musicVolume;
        }
        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
        }

        // Panggil fungsi set volume untuk memastikan mixer diupdate di awal
        SetMusicVolume(musicVolume);
        SetSFXVolume(sfxVolume);
    }

    /// <summary>
    /// Panggil fungsi ini saat game ditutup untuk memastikan data tersimpan.
    /// </summary>
    private void OnApplicationQuit()
    {
        PlayerPrefs.Save();
        Debug.Log("Pengaturan volume disimpan!");
    }
}