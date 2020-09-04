using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuSaveManager : MonoBehaviour
{
    [SerializeField] private Button continueButton = null;

    private void Start()
    {
        if (continueButton)
        {
            if (GeneralFunctions.IsAnySaveSlotActive())
            {
                continueButton.interactable = true;
            }
            else
            {
                continueButton.interactable = false;
            }
        }
    }
    /// <summary>
    /// Loads the current active save file
    /// </summary>
    public void LoadActiveSave()
    {
        GeneralFunctions.LoadActiveSave();
    }
    /// <summary>
    /// Closes the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}