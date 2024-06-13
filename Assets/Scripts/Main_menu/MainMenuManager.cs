using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void LoadGame()
    {
        SceneManager.LoadScene(1); // Game Scene
    }
    public void ExitAplication()
    {
        Application.Quit(); //Exit
    }
    public void ControlsMenu()
    {
        SceneManager.LoadScene(2); // Control Scene
    }
}
