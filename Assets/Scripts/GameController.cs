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
    public int score = 0;
    public List<LevelParameters> levels;
    private string saveFilePath;
    private GameProgress progress;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
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
            progress = JsonUtility.FromJson<GameProgress>(json);
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

    void InitializeFreshGame()
    {
        progress = new GameProgress
        {
            level = 1,
            score = 0
        };
    }

    public void StartLevel()
    {
        currentState = GameState.Playing;
        score = 0;
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

    public void LevelFinished()
    {
        if(progress.level < levels.Count)
        {
            progress.level++;
        }
        SaveGame();
        currentState = GameState.MainMenu;
        SceneManager.LoadScene("MainMenu");
    }

    public void AddScore(int points)
    {
        if (currentState == GameState.Playing)
        {
            score += points;
        }
    }

    public LevelParameters GetLevelParameters()
    {
        return levels[progress.level - 1];
    }
}
