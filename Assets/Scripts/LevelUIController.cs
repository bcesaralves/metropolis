using TMPro;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboScoreText;
    public TMP_Text levelText;
    public TMP_Text timerText;

    private void Awake()
    {
        LevelController levelController = LevelController.instance.GetComponent<LevelController>();
        if (levelController != null)
        {
            levelController.OnScoreChanged += OnScoreChanged;
            levelController.OnComboScoreChanged += OnComboScoreChanged;
            levelController.OnTimeChanged += OnTimeChanged;
        }
    }

    private void Start()
    {
        LevelController levelController = LevelController.instance.GetComponent<LevelController>();
        if (levelController != null)
        {
            if (levelText)
            {
                levelText.text = "Level: " + levelController.GetLevel().ToString();
            }
        }
    }

    public void OnScoreChanged(int score)
    {
        if (scoreText)
        {
            scoreText.text = "Level Score: " + score.ToString();
        }
    }

    public void OnComboScoreChanged(int combo)
    {
        if (comboScoreText)
        {
            comboScoreText.text = "Combo Score: " + combo.ToString();
        }
    }

    public void OnTimeChanged(int remainingTime)
    {
        if (timerText)
        {
            timerText.text = "Remaining Time: " + remainingTime.ToString();
        }
    }

    void OnDestroy()
    {
        if (LevelController.instance)
        {
            LevelController.instance.GetComponent<LevelController>().OnScoreChanged -= OnScoreChanged;
            LevelController.instance.GetComponent<LevelController>().OnComboScoreChanged -= OnComboScoreChanged;
            LevelController.instance.GetComponent<LevelController>().OnTimeChanged -= OnTimeChanged;
        }
    }
}
