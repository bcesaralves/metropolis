using System;
using UnityEngine;

public class ClickableObject : MonoBehaviour
{
    public Action OnClick;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }
        else if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                HandleClick(touch.position);
            }
        }
    }

    void HandleClick(Vector3 inputPosition)
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                OnClick?.Invoke();
            }
        }
    }
}