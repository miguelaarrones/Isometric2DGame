using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1); // The game scene, as the main menu is scene 0.npuit
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
