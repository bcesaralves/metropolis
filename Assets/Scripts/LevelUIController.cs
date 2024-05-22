using TMPro;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text levelText;

    private void Awake()
    {
        LevelController levelController = LevelController.instance.GetComponent<LevelController>();
        if (levelController != null)
        {
            levelController.OnScoreChanged += OnScoreChanged;
        }
    }

    private void Start()
    {
        LevelController levelController = LevelController.instance.GetComponent<LevelController>();
        if (levelController != null)
        {
            if (levelText)
            {
                levelText.text = "Level " + levelController.GetLevel().ToString();
            }
        }
    }

    public void OnScoreChanged(int score)
    {
        if (scoreText)
        {
            scoreText.text = "Level Score " + score.ToString();
        }
    }

    void OnDestroy()
    {
        if (LevelController.instance)
        {
            LevelController.instance.GetComponent<LevelController>().OnScoreChanged -= OnScoreChanged;
        }
    }
}
