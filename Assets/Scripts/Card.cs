using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public enum CardState
    {
        Hidden,
        Revealing,
        Revealed,
        Hiding
    }

    public Action<Card> OnRevealed;

    public float revealDuration = 0.5f; // Duration of the reveal animation in seconds
    public float hideDuration = 0.5f;   // Duration of the hide animation in seconds

    public int cardID = -1;
    private CardState currentState = CardState.Hidden;
    private Quaternion hiddenRotation = Quaternion.Euler(0f, 180f, 0f);
    private Quaternion revealedRotation = Quaternion.Euler(0f, 0f, 0f);

    private void Awake()
    {
        GetComponent<ClickableObject>().OnClick += HandleClick;
    }

    private void Start()
    {
        currentState = CardState.Hidden;
        transform.rotation = hiddenRotation;
    }

    public void SetCardID(int cardID)
    {
        this.cardID = cardID;
    }

    public void Reveal()
    {
        if (currentState != CardState.Hidden)
            return;

        currentState = CardState.Revealing;
        StartCoroutine(AnimateRotation(revealDuration, revealedRotation));
    }

    public void Hide()
    {
        if (currentState != CardState.Revealed)
            return;

        currentState = CardState.Hiding;
        StartCoroutine(AnimateRotation(hideDuration, hiddenRotation));
    }

    private IEnumerator AnimateRotation(float duration, Quaternion targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        float timeElapsed = 0f;

        while (timeElapsed < duration)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / duration);
            transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);
            yield return null;
        }

        transform.rotation = targetRotation;

        if (targetRotation == revealedRotation)
        {
            currentState = CardState.Revealed;
            OnRevealed?.Invoke(this);
        }
        else if (targetRotation == hiddenRotation)
        {
            currentState = CardState.Hidden;
        }
    }

    private void HandleClick()
    {
        Reveal();
    }

    void OnDestroy()
    {
        GetComponent<ClickableObject>().OnClick -= HandleClick;
    }
}