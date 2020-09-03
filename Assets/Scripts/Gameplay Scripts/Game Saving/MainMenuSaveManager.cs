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
    /// Loads the sandbox test level
    /// </summary>
    public void PlayGame()
    {
        SceneManager.LoadScene("Sandbox");
    }
    /// <summary>
    /// Closes the game
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}