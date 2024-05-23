using System.Collections.Generic;
using UnityEngine;

public class CardArranger : MonoBehaviour
{
    public float horizontalMargin; // Horizontal margin between cards
    public float verticalMargin; // Vertical margin between cards
    public enum ArrangeType { Screen, Container};
    private List<Card> cards;
    private Transform _transform;

    private void Awake()
    {
        cards = new List<Card>();
        _transform = GetComponent<Transform>();
    }

    public void Arrange(ArrangeType type, int rows, int columns)
    {

        if(rows * columns %  2 != 0)
        {
            Debug.LogError("Number of cards must by even.");
            return;
        }
        
        GameController gameController = GameController.instance.GetComponent<GameController>();

        List<int> cardsIDs = new List<int>();
        for (int i = 1; i <= rows * columns / 2; i++)
        {
            cardsIDs.Add(i);
        }
        for (int i = 1; i <= rows * columns / 2; i++)
        {
            cardsIDs.Add(i);
        }
        Shuffle(cardsIDs);


        float containerWidth;
        float containerHeight;
        float containerX;
        float containerY;

        switch (type)
        {
            case ArrangeType.Screen:
                containerWidth = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
                containerHeight = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);
                containerX = containerY = 0;
                break;
            case ArrangeType.Container:
                containerWidth = _transform.localScale.x;
                containerHeight = _transform.localScale.y;
                containerX = _transform.position.x;
                containerY = _transform.position.y;
                break;
            default:
                containerWidth = _transform.localScale.x;
                containerHeight = _transform.localScale.y;
                containerX = containerY = 0;
                break;
        }



        float totalHorizontalMargin = 2 * (columns + 1) * horizontalMargin;
        float totalVerticalMargin = 2 * (rows + 1) * verticalMargin;

        // Calculate the size of each card

        float maxCardWidth = (containerWidth - totalHorizontalMargin) / columns;
        float maxCardHeight = (containerHeight - totalVerticalMargin) / rows;

        float cardAspectRatio = gameController.GetCardsAspectRatio();

        // Adjust the card size to maintain the aspect ratio
        float cardWidth, cardHeight;
        if (maxCardWidth / maxCardHeight > cardAspectRatio)
        {
            cardHeight = maxCardHeight;
            cardWidth = cardHeight * cardAspectRatio;
        }
        else
        {
            cardWidth = maxCardWidth;
            cardHeight = cardWidth / cardAspectRatio;
        }

        // Instantiate and position the cards

        float currentY = containerY + 2 * verticalMargin - containerHeight / 2 + maxCardHeight / 2;
        int cardIndex = 0;
        for (int i = 0; i < rows; i++)
        {
            float currentX = containerX + 2 * horizontalMargin - containerWidth / 2 + maxCardWidth / 2;
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(currentX,currentY,0);

                GameObject cardPrefab = gameController.GetCard(cardsIDs[cardIndex]);
                if (cardPrefab == null)
                {
                    Debug.LogError("Card prefab is not correctly assigned.");
                    continue;
                }
                GameObject card = Instantiate(cardPrefab, transform.parent);
                Transform cardTransform = card.GetComponent<Transform>();
                Card cardComponent = card.GetComponent<Card>();
                cardTransform.localScale = new Vector3(cardWidth, cardHeight, 1);
                cardTransform.localPosition = position;
                cardComponent.OnRevealed += LevelController.instance.HandleCardReveal;
                cardComponent.SetCardID(cardsIDs[cardIndex]);
                cards.Add(cardComponent);
                cardIndex++;
                currentX += maxCardWidth + 2 * horizontalMargin;
            }
            currentY += maxCardHeight + 2 * verticalMargin;
        }
    }
    private void Shuffle(List<int> list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    void OnDestroy()
    {
        foreach(Card card in cards)
        {
            card.OnRevealed -= LevelController.instance.HandleCardReveal;
        }
    }
}