using TMPro;
using UnityEngine;

public class GameUIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text levelText;

    void Awake()
    {
        GameController gameController = GameController.instance.GetComponent<GameController>();
        if (gameController != null)
        {
            gameController.OnScoreChanged += OnScoreChanged;
            gameController.OnLevelChanged += OnLevelChanged;
        }
    }

    public void OnScoreChanged(int score)
    {
        if(scoreText)
        {
            scoreText.text = "Score: " + score.ToString();
        }
    }

    public void OnLevelChanged(int level)
    {
        if (levelText)
        {
            levelText.text = "Next Level: " + level.ToString();
        }
    }

    void OnDestroy()
    {
        if(GameController.instance)
        {
            GameController gameController = GameController.instance.GetComponent<GameController>();
            gameController.OnScoreChanged -= OnScoreChanged;
            gameController.OnLevelChanged -= OnLevelChanged;
        }
    }
}
