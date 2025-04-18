using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 50;
    public Text scoreText;
    public Text timerText;
    public float startTime = 60f;

    private float currentTime;
    private bool isTimerRunning = false;

    public AudioSource backgroundAudioSource;
    public AudioSource winAudioSource;
    public AudioSource loseAudioSource;

    private bool isGameOver = false;

    void Start()
    {
        currentTime = startTime;
        StartTimer();
        UpdateScoreUI();
        PlayBackgroundMusic();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                StopTimer();
                if (!isGameOver)
                {
                    GameOver();
                }
            }
            UpdateTimerText();
        }

        if (score >= targetScore && !isGameOver)
        {
            WinGame();
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    public void StartTimer()
    {
        isTimerRunning = true;
    }

    public void StopTimer()
    {
        isTimerRunning = false;
    }

    public void ResetTimer()
    {
        currentTime = startTime;
        StartTimer();
    }

    void UpdateTimerText()
    {
        int seconds = Mathf.FloorToInt(currentTime);
        timerText.text = "Timer: " + seconds;
    }

    void GameOver()
    {
        isGameOver = true;
        StopAllAudio();
        if (loseAudioSource != null)
        {
            loseAudioSource.Play();
        }
        Invoke(nameof(ReloadScene), loseAudioSource.clip.length);
    }

    void WinGame()
    {
        isGameOver = true;
        StopAllAudio();
        if (winAudioSource != null)
        {
            winAudioSource.Play();
        }
        Invoke(nameof(LoadNextLevel), winAudioSource.clip.length);
    }

    private void PlayBackgroundMusic()
    {
        if (backgroundAudioSource != null)
        {
            backgroundAudioSource.loop = true;
            backgroundAudioSource.Play();
        }
    }

    private void StopAllAudio()
    {
        if (backgroundAudioSource != null && backgroundAudioSource.isPlaying)
        {
            backgroundAudioSource.Stop();
        }
        if (winAudioSource != null && winAudioSource.isPlaying)
        {
            winAudioSource.Stop();
        }
        if (loseAudioSource != null && loseAudioSource.isPlaying)
        {
            loseAudioSource.Stop();
        }
    }

    private void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}