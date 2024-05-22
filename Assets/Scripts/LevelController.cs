using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance = null;
    public CardArranger arranger;
    // Example variable to hold level parameters received from GameController
    private LevelParameters levelParams;
    private Queue<Card> selectedCards = new Queue<Card>();
    private int unrevealedCards;

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

        // Retrieve level parameters from GameController
        if(GameController.instance)
        {
            levelParams = GameController.instance.GetLevelParameters();
        } else
        {
            levelParams = new LevelParameters();
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

    public void HandleCardReveal(Card card)
    {
        print(card.cardID);
        selectedCards.Enqueue(card);
        ProcessQueue();
    }

    private void ProcessQueue()
    {
        if(selectedCards.Count >= 2)
        {
            Card firstCard = selectedCards.Dequeue();
            Card secondCard = selectedCards.Dequeue();

            if(firstCard.cardID == secondCard.cardID)
            {
                //match
                unrevealedCards -= 2;
                CheckEndLevel();
            } else
            {
                firstCard.Hide();
                secondCard.Hide();
            }
        }
    }
    private void CheckEndLevel()
    {
        if(unrevealedCards < 0)
        {
            Debug.LogError("number of matched cards exceeded");
        }
        if (unrevealedCards == 0)
        {
            GameController.instance.LevelFinished(0);
        }
    }
}