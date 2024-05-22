using UnityEngine;

public class PlayButton : MonoBehaviour
{
    public void Click()
    {
        GameController.instance.StartLevel();
    }
}
