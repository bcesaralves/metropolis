using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance = null;
    public CardArranger.ArrangeType arrangeType;
    public enum GameState { MainMenu, Playing, GameOver };
    public GameState currentState = GameState.MainMenu;
    public int score = 0;
    private LevelParameters levelParameters;

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

    void Update()
    {

    }

    public void StartGame()
    {
        currentState = GameState.Playing;
        score = 0;
        levelParameters = new LevelParameters();
        levelParameters.horizontalNumberOfCards = 6;
        levelParameters.verticalNumberOfCards = 4;
        levelParameters.type = arrangeType;
        SceneManager.LoadScene("Game");
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

    public void AddScore(int points)
    {
        if (currentState == GameState.Playing)
        {
            score += points;
        }
    }

    public LevelParameters GetLevelParameters()
    {
        return levelParameters;
    }
}
