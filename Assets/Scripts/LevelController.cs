using UnityEngine;

public class LevelController : MonoBehaviour
{
    public CardArranger arranger;
    // Example variable to hold level parameters received from GameController
    private LevelParameters levelParams;

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