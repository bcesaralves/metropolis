using UnityEngine;

public class ResetButton : MonoBehaviour
{
    public void Click()
    {
        GameController.instance.ResetGame();
    }
}
