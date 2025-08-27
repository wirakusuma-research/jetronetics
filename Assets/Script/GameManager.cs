using UnityEngine;
using UnityEngine.UI; // Menggunakan namespace UI
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Pola Singleton untuk akses mudah
    public static GameManager Instance;

    [Header("Score")]
    public int currentScore = 0;
    public int highscore = 0;

    [Header("UI References")]
    public Text scoreText;
    public Text finalScoreText;
    public Text highscoreText;

    [Header("Audio")]
    [Tooltip("Tag dari audio yang ingin ikut di-pause.")]
    public string musicTag = "Music";

    private const string HighscoreKey = "HighScore";
    private bool isGamePaused = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }

    void Start()
    {
        // Muat highscore yang disimpan
        LoadHighscore();
        UpdateScoreUI();
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        UpdateScoreUI();
        CheckForHighscore();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + currentScore;
        }
        if(finalScoreText != null)
        {
            finalScoreText.text = "Score: " + currentScore;
        }
        if (highscoreText != null)
        {
            highscoreText.text = "Highscore: " + highscore;
        }
    }

    private void CheckForHighscore()
    {
        if (currentScore > highscore)
        {
            highscore = currentScore;
            SaveHighscore();
            UpdateScoreUI();
        }
    }

    private void SaveHighscore()
    {
        // Menyimpan highscore menggunakan PlayerPrefs
        PlayerPrefs.SetInt(HighscoreKey, highscore);
        PlayerPrefs.Save();
        Debug.Log("Highscore baru disimpan: " + highscore);
    }

    private void LoadHighscore()
    {
        // Memuat highscore yang disimpan, jika ada
        if (PlayerPrefs.HasKey(HighscoreKey))
        {
            highscore = PlayerPrefs.GetInt(HighscoreKey);
        }
    }

    // Fungsi untuk me-reset skor saat game berakhir (jika diperlukan)
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        isGamePaused = true;

        // Cari semua GameObject dengan tag "Music"
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag(musicTag);
        foreach (GameObject musicObject in musicObjects)
        {
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.Pause();
            }
        }

        Debug.Log("Game di-pause!");
    }

    /// <summary>
    /// Mengatur game ke mode resume dan melanjutkan semua audio yang di-pause.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        isGamePaused = false;

        // Cari semua GameObject dengan tag "Music"
        GameObject[] musicObjects = GameObject.FindGameObjectsWithTag(musicTag);
        foreach (GameObject musicObject in musicObjects)
        {
            AudioSource audioSource = musicObject.GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.UnPause();
            }
        }

        Debug.Log("Game di-resume!");
    }
}
