using TMPro;
using UnityEngine;

public class LevelUIController : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text comboScoreText;
    public TMP_Text levelText;
    public TMP_Text timerText;
    private LevelController _levelController;

    private void Awake()
    {
        if(LevelController.instance)
        {
            _levelController = LevelController.instance.GetComponent<LevelController>();
            _levelController.OnScoreChanged += OnScoreChanged;
            _levelController.OnComboScoreChanged += OnComboScoreChanged;
            _levelController.OnTimeChanged += OnTimeChanged;
        }
    }

    private void Start()
    {
        if (_levelController != null)
        {
            if (levelText)
            {
                levelText.text = "Level: " + _levelController.GetLevel().ToString();
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
        if (_levelController)
        {
            _levelController.OnScoreChanged -= OnScoreChanged;
            _levelController.OnComboScoreChanged -= OnComboScoreChanged;
            _levelController.OnTimeChanged -= OnTimeChanged;
        }
    }
}
