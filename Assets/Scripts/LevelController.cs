using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance = null;
    public CardArranger arranger;
    public int scorePerMatch;
    public int scorePerCombo;
    public int sequenceForCombo;
    // Example variable to hold level parameters received from GameController
    private LevelParameters levelParams;
    private Queue<Card> selectedCards = new Queue<Card>();
    private int unrevealedCards;
    public Action<int> OnScoreChanged;
    public Action<int> OnComboScoreChanged;
    private int score;
    private int combo;
    private int sequence;

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
    }

    private void Start()
    {

        if (arranger == null)
        {
            Debug.LogError("Arranger is not assigned.");
            return;
        }

        SetScore(0,0);

        // Retrieve level parameters from GameController
        if (GameController.instance)
        {
            levelParams = GameController.instance.GetLevelParameters();
        } else
        {
            levelParams = new LevelParameters();
            levelParams.level = 1;
            levelParams.horizontalNumberOfCards = 4;
            levelParams.verticalNumberOfCards = 2;
            levelParams.type = CardArranger.ArrangeType.Screen;
        }
        unrevealedCards = levelParams.horizontalNumberOfCards * levelParams.verticalNumberOfCards;
        if (unrevealedCards % 2 != 0)
        {
            Debug.LogError("Number of cards must be even");
            return;
        }
        
        arranger.Arrange(levelParams.type,levelParams.verticalNumberOfCards,levelParams.horizontalNumberOfCards);

    }

    public int GetLevel()
    {
        return levelParams.level;
    }

    public void HandleCardReveal(Card card)
    {
        print(card.cardID);
        selectedCards.Enqueue(card);
        ProcessQueue();
    }


    public void SetScore(int score, int combo)
    {
        this.score = score;
        this.combo = combo;
        OnScoreChanged?.Invoke(score);
        OnComboScoreChanged?.Invoke(combo);
    }

    public void UpdateScore(int scoreChange, int comboChange)
    {
        score += scoreChange;
        combo += comboChange;
        OnScoreChanged?.Invoke(score);
        OnComboScoreChanged?.Invoke(combo);
    }

    private void ProcessQueue()
    {
        if(selectedCards.Count >= 2)
        {
            Card firstCard = selectedCards.Dequeue();
            Card secondCard = selectedCards.Dequeue();

            if(firstCard.cardID == secondCard.cardID)
            {
                Match();
            } else
            {
                firstCard.Hide();
                secondCard.Hide();
                Fail();
            }
        }
    }

    private void Match()
    {
        sequence++;
        if (sequence >= sequenceForCombo)
        {
            UpdateScore(scorePerMatch, scorePerCombo);
            sequence = 0;
        }
        else
        {
            UpdateScore(scorePerMatch, 0);
        }
        unrevealedCards -= 2;
        SoundController.Instance.PlaySoundEffect("matching");
        CheckEndLevel();
    }

    private void Fail()
    {
        sequence = 0;
        SoundController.Instance.PlaySoundEffect("mismatching");
    }


    private void CheckEndLevel()
    {
        if(unrevealedCards < 0)
        {
            Debug.LogError("number of matched cards exceeded");
        }
        if (unrevealedCards == 0)
        {
            GameController.instance.LevelFinished(score + combo);
        }
    }
}