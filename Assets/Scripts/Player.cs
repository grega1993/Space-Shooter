using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed = 6.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleLaserPrefab;
    [SerializeField]
    private float _fireRate = 0.5f;
    private float _canFire = 0.0f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private bool isTripleshotActive = false;
    [SerializeField]
    private bool isShieldActive = false;
    private SpawnManager _spawnManager;
    [SerializeField]
    private GameObject _ShieldVisualizer;
    [SerializeField]
    private int _score = 0;
    [SerializeField]
    private GameObject _leftEngine;
    [SerializeField]
    private GameObject _rightEngine;
    private UIManager _uiManager;
    //store audio clip
    [SerializeField]
    private AudioClip _laserSoundClip;
    [SerializeField]
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        // Set the initial position of the player to the origin (0,0,0)
        transform.position = Vector3.zero;

        // Find and assign the SpawnManager component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        // Find and assign the UIManager component
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        // Check if the SpawnManager was successfully assigned
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn manager is NULL");
        }

        // Check if the UIManager was successfully assigned
        if (_uiManager == null)
        {
            Debug.LogError("The UI manager is NULL");
        }

        // Check if the AudioSource was successfully assigned
        if (_audioSource == null)
        {
            Debug.LogError("Audio Source is NULL");
        }
        else
        {
            // Set the audio clip to the laser sound
            _audioSource.clip = _laserSoundClip;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Handle player movement
        CalculateMovement();

        // If the space key is pressed and the fire cooldown has elapsed, fire a laser
        if (Input.GetKey(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }
    }
    void CalculateMovement()
    {
        // Get the horizontal and vertical input values
        float HorizontalInput = Input.GetAxis("Horizontal");
        float VerticalInput = Input.GetAxis("Vertical");

        // Create a direction vector based on the input
        Vector3 Direction = new Vector3(HorizontalInput, VerticalInput, 0);

        // Move the player in the direction of the input
        transform.Translate(Direction * _speed * Time.deltaTime);

        // Clamp the player's vertical position to stay within the screen bounds
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -6.2f, 6.2f), 0);

        // Wrap the player's horizontal position if they move off the screen
        if (transform.position.x >= 16.21f)
        {
            transform.position = new Vector3(-16.20f, transform.position.y, 0);
        }
        else if (transform.position.x <= -16.21f)
        {
            transform.position = new Vector3(16.20f, transform.position.y, 0);
        }
    }

    void FireLaser()
    {
        // Set the next allowed fire time based on the current time and fire rate
        _canFire = Time.time + _fireRate;

        // Fire a triple shot if the triple shot power-up is active
        if (isTripleshotActive == true)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
        }
        else
        {
            // Fire a single laser
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }

        // Play the laser sound
        _audioSource.Play();
    }

    public void Damage()
    {
        // If the shield is active, deactivate it and return
        if (isShieldActive == true)
        {
            isShieldActive = false;
            _ShieldVisualizer.SetActive(false);
            return;
        }
        else
        {
            // Reduce the player's lives by 1
            _lives -= 1;

            // Activate the left engine damage visual if lives are 2
            if (_lives == 2)
            {
                _leftEngine.SetActive(true);
            }
            // Activate the right engine damage visual if lives are 1
            else if (_lives == 1)
            {
                _rightEngine.SetActive(true);
            }

            // Update the UI with the new number of lives
            _uiManager.updateLives(_lives);

            // If lives are 0 or less, notify the SpawnManager and destroy the player object
            if (_lives <= 0)
            {
                _spawnManager.onPlayerDeath();
                Destroy(gameObject);
            }
        }
    }

    public void TripleShotActive()
    {
        // Activate the triple shot power-up
        isTripleshotActive = true;

        // Start a coroutine to deactivate the power-up after a delay
        StartCoroutine(PowerDownTripleshotRoutine());
    }

    IEnumerator PowerDownTripleshotRoutine()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5);

        // Deactivate the triple shot power-up
        isTripleshotActive = false;
    }

    public void Speedboost()
    {
        // Increase the player's speed
        _speed = _speed + 4;

        // Decrease the fire rate if it's above a certain threshold
        if (_fireRate > 0.06f)
        {
            _fireRate = _fireRate - 0.02f;
        }

        // Start a coroutine to reset the speed after a delay
        StartCoroutine(SpeedboostPowerDown());
    }

    IEnumerator SpeedboostPowerDown()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5);

        // Reset the player's speed to the default value
        _speed = 6.0f;
    }

    public void ShieldActive()
    {
        // Activate the shield visualizer and set the shield to active
        _ShieldVisualizer.SetActive(true);
        isShieldActive = true;
    }

    // Add points to the score and update the UI
    public void AddScore(int points)
    {
        _score = _score + points;
        _uiManager.updateScore(_score);
    }

    // Return the current score
    public int Score()
    {
        return _score;
    }
}