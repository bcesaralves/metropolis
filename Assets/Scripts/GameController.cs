using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public CardArranger.ArrangeType arrangeType;
    public enum GameState { MainMenu, Playing, GameOver };
    public GameState currentState = GameState.MainMenu;
    public List<LevelParameters> levels;
    private string saveFilePath;
    private GameProgress progress;
    public Action<int> OnScoreChanged;
    public Action<int> OnLevelChanged;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
        LoadGame();
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            SetProgress(JsonUtility.FromJson<GameProgress>(json));
            Debug.Log("Game loaded successfully.");
        }
        else
        {
            Debug.Log("Initializing fresh game.");
            InitializeFreshGame();
        }
    }

    public void SaveGame()
    {
        string json = JsonUtility.ToJson(progress, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game saved successfully.");
    }

    public void ResetGame()
    {
        InitializeFreshGame();
        SaveGame();
    }

    void InitializeFreshGame()
    {
        GameProgress progress = new GameProgress
        {
            level = 1,
            score = 0
        };
        SetProgress(progress);
    }

    public void StartLevel()
    {
        currentState = GameState.Playing;
        SceneManager.LoadScene("Level");
    }

    public void GameOver()
    {
        currentState = GameState.GameOver;
        SceneManager.LoadScene("GameOver");
    }

    public void RestartGame()
    {
        currentState = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    public void LevelFinished(int levelScore)
    {
        GameProgress progressChange = new GameProgress { score = levelScore };
        if (progress.level < levels.Count)
        {
            progressChange.level = 1;
        }
        UpdateProgress(progressChange);
        SaveGame();
        currentState = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    public LevelParameters GetLevelParameters()
    {
        return levels[progress.level - 1];
    }

    private void SetProgress(GameProgress progress)
    {
        this.progress = progress;
        OnLevelChanged?.Invoke(progress.level);
        OnScoreChanged?.Invoke(progress.score);
    }

    public void UpdateProgress(GameProgress progressChange)
    {
        if(progressChange.level > 0)
        {
            progress.level += progressChange.level;
            OnLevelChanged?.Invoke(progress.level);
        }
        if (progressChange.score > 0)
        {
            progress.score += progressChange.score;
            OnScoreChanged?.Invoke(progress.score);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(progress != null)
        {
            OnLevelChanged?.Invoke(progress.level);
            OnScoreChanged?.Invoke(progress.score);
        }
        
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
