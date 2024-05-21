using System.Collections.Generic;
using UnityEngine;

public class CardArranger : MonoBehaviour
{
    public GameObject cardPrefab; // The card prefab to instantiate
    public int rows; // Number of rows
    public int columns; // Number of columns
    public float horizontalMargin; // Horizontal margin between cards
    public float verticalMargin; // Vertical margin between cards
    public enum ArrangeType { Screen, Container}; 
    ArrangeType arrangeType;
    void Start()
    {
        Arrange(ArrangeType.Screen);
    }

    public void Arrange(ArrangeType type)
    {
        if (cardPrefab == null)
        {
            Debug.LogError("Card prefab is not assigned.");
            return;
        }

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

        float cardAspectRatio = cardPrefab.GetComponent<SpriteRenderer>().bounds.size.x / cardPrefab.GetComponent<SpriteRenderer>().bounds.size.y;

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
        for (int i = 0; i < rows; i++)
        {
            float currentX = 2 * horizontalMargin - containerWidth / 2 + maxCardWidth / 2;
            for (int j = 0; j < columns; j++)
            {
                Vector3 position = new Vector3(currentX,currentY,0);

                GameObject card = Instantiate(cardPrefab, transform.parent);

                card.GetComponent<Transform>().localScale = new Vector3(cardWidth, cardHeight, 1);
                card.GetComponent<Transform>().localPosition = position;
                if(type == ArrangeType.Container)
                {
                    card.GetComponent<Transform>().parent = GetComponent<Transform>();
                }
                currentX += maxCardWidth + 2 * horizontalMargin;
            }
            currentY += maxCardHeight + 2 * verticalMargin;
        }
    }
}