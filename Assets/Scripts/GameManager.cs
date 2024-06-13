using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private bool _isGameOver=false;
    [SerializeField]
    private GameObject _pauseGameMenuPanel;
    private Animator _animatorPause;
    private void Start()
    {
        // Find the Pause Menu Panel and get its Animator component
        _animatorPause = GameObject.Find("Pause_menu_panel").GetComponent<Animator>();

        // Set the animator's update mode to UnscaledTime to work with paused time
        _animatorPause.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        // Check if the game is over and if the R key is pressed to reload the game scene
        if (_isGameOver == true && Input.GetKeyDown(KeyCode.R))
        {
            _isGameOver = false;
            SceneManager.LoadScene(1); // Load the Game Scene
        }

        // Check if the Escape key is pressed to quit the application
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Check if the P key is pressed to pause the game
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 1f)
        {
            // Activate the pause menu panel and set the isPaused parameter in the animator to true
            _pauseGameMenuPanel.SetActive(true);
            _animatorPause.SetBool("isPaused", true);

            // Pause the game by setting time scale to 0
            Time.timeScale = 0f;
        }
        // Check if the P key is pressed again to resume the game
        else if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0f)
        {
            // Resume the game by setting time scale to 1 and deactivate the pause menu panel
            Time.timeScale = 1f;
            _pauseGameMenuPanel.SetActive(false);
        }
    }

    // Method to deactivate the pause panel and resume the game
    public void DeactivatePausePanelandResume()
    {
        _pauseGameMenuPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    // Method to set the game over flag to true
    public void GameOver()
    {
        _isGameOver = true;
    }
}
