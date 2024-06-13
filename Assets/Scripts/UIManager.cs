using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private Text _scoreText;
    [SerializeField]
    private Image _livesImg;
    [SerializeField]
    private Sprite[] _liveSprites;
    [SerializeField]
    private Text _gameOverVisualizer;
    [SerializeField]
    private Text _restartTextVisualizer;
    [SerializeField]
    private GameManager _gameManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the score text to show a score of 0
        _scoreText.text = "Score: " + 0;

        // Hide the game over visualizer at the start
        _gameOverVisualizer.gameObject.SetActive(false);

        // Find the GameManager and get its component
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        // Null check for the GameManager
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic can be added here if needed
    }

    // Update the score text with the player's current score
    public void updateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    // Update the lives display based on the current lives
    public void updateLives(int currentLives)
    {
        _livesImg.sprite = _liveSprites[currentLives];

        // If no lives are left, initiate the game over sequence
        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    // Handle the game over sequence
    void GameOverSequence()
    {
        // Notify the GameManager that the game is over
        _gameManager.GameOver();

        // Show the game over visualizer and the restart text
        _gameOverVisualizer.gameObject.SetActive(true);
        _restartTextVisualizer.gameObject.SetActive(true);

        // Start the game over flicker routine
        StartCoroutine(GameOverFlickerRoutine());
    }

    // Coroutine to flicker the game over text
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            // Show the game over text
            _gameOverVisualizer.text = "GAME OVER";
            yield return new WaitForSeconds(0.5f);

            // Hide the game over text
            _gameOverVisualizer.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Resume the game from pause
    public void ResumePlay()
    {
        // Notify the GameManager to deactivate the pause panel and resume the game
        _gameManager.DeactivatePausePanelandResume();
    }

    // Go back to the main menu
    public void BackToMainmenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene(0);
    }

}
