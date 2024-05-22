using UnityEngine;

public class PlayButton : MonoBehaviour
{
    // Start is called before the first frame update
    public void Click()
    {
        GameController.instance.StartGame();
    }
}
