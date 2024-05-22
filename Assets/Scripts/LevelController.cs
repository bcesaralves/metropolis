using UnityEngine;

public class LevelController : MonoBehaviour
{
    public static LevelController instance = null;
    public CardArranger arranger;
    // Example variable to hold level parameters received from GameController
    private LevelParameters levelParams;

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
        levelParams = GameController.instance.GetLevelParameters();
        arranger.Arrange(levelParams.type,levelParams.verticalNumberOfCards,levelParams.horizontalNumberOfCards);

    }
}