using System;
using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private static int levelNumber = 1;
    [SerializeField] private List<GameLevel> gameLevels;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    private int score = 0;
    private float timer;
    private bool isTimerActive;
    private static int totalScore = 0;
    public static void ResetGame()
    {
        levelNumber = 1;
        totalScore = 0;
    }


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
        Lander.Instance.OnStateChanged += Lander_OnStateChanged;
        LoadCurrentLevel();
    }
    private void LoadCurrentLevel()
    {
        GameLevel currentLevel = GetGameLevel();
        GameLevel spawnedGameLevel = Instantiate(currentLevel, Vector3.zero, Quaternion.identity);
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTarget();
        CinemachineCameraZoom2D.Instance.SetTargetOrthographicSize(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }
    private GameLevel GetGameLevel()
    {
        foreach (GameLevel level in gameLevels)
        {
            if (level.GetLevelNumber() == levelNumber)
            {
                return level;
            }
        }
        return null;
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
    {
        isTimerActive = e.state == Lander.State.Normal;
        if (e.state == Lander.State.Normal)
        {
            cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform;
            CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize();
        }
    }

    private void Update()
    {
        if (isTimerActive)
        {
            timer += Time.deltaTime;
        }
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, EventArgs e)
    {
        AddScore(100);
    }
    public void AddScore(int scoretoAdd)
    {
        score += scoretoAdd;
        Debug.Log($"Score: {score}");
    }
    public int GetScore()
    {
        return score;
    }
    public float GetTime()
    {
        return timer;
    }
    public void GoToNextLevel()
    {
        levelNumber++;
        totalScore += score;
        if (GetGameLevel() == null)
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);

        }
        else
        {
            SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
        }

    }
    public void RestartLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameScene);
    }
    public int GetCurrentLevelNumber()
    {
        return levelNumber;
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnpauseGame()
    {
        Time.timeScale = 1f;
    }
    public void PauseUnpause()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
            PausedUI.instance.Show();
        }
        else
        {
            UnpauseGame();
            PausedUI.instance.Hide();
        }
    }
    public int GetTotalScore()
    {
        return totalScore;
    }
}
