using System.Collections.Generic;
using UnityEngine;

public class CardArranger : MonoBehaviour
{
    public float horizontalMargin; // Horizontal margin between cards
    public float verticalMargin; // Vertical margin between cards
    public enum ArrangeType { Screen, Container};
    private List<Card> cards;

    private void Awake()
    {
        cards = new List<Card>();
    }

    public void Arrange(ArrangeType type, int rows, int columns)
    {


        //TODO: check if is even
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

        switch (type)
        {
            case ArrangeType.Screen:
                containerWidth = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).x - 0.5f);
                containerHeight = 1 / (Camera.main.WorldToViewportPoint(new Vector3(1, 1, 0)).y - 0.5f);
                break;
            case ArrangeType.Container:
                containerWidth = GetComponent<Transform>().localScale.x;
                containerHeight = GetComponent<Transform>().localScale.y;
                break;
            default:
                containerWidth = GetComponent<Transform>().localScale.x;
                containerHeight = GetComponent<Transform>().localScale.y;
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

        float currentY = 2 * verticalMargin - containerHeight / 2 + maxCardHeight / 2;
        int cardIndex = 0;
        for (int i = 0; i < rows; i++)
        {
            float currentX = 2 * horizontalMargin - containerWidth / 2 + maxCardWidth / 2;
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
                card.GetComponent<Transform>().localScale = new Vector3(cardWidth, cardHeight, 1);
                card.GetComponent<Transform>().localPosition = position;
                card.GetComponent<Card>().OnRevealed += LevelController.instance.HandleCardReveal;
                card.GetComponent<Card>().SetCardID(cardsIDs[cardIndex]);
                cards.Add(card.GetComponent<Card>());
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