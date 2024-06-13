using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _speed = 4.0f;
    [SerializeField]
    private GameObject _EnemylaserPrefab;
    private Player _player;
    private Animator _animator;
    private float _fireRate = 3.0f;
    private float _canFire = -1;
    private AudioSource _audioSource;


    // Start is called before the first frame update
    void Start()
    {
        // Find and assign the Player component
        _player = GameObject.Find("Player").GetComponent<Player>();
        // Assign the AudioSource component
        _audioSource = GetComponent<AudioSource>();

        // Null check for the player to ensure it is properly assigned
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }

        // Assign the Animator component
        _animator = GetComponent<Animator>();

        // Null check for the animator to ensure it is properly assigned
        if (_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Calculate the enemy's movement
        CalculateMovement();

        // Check if the enemy can fire and the player's score is at least 20
        if (Time.time > _canFire && _player.Score() >= 20)
        {
            // Set a random fire rate
            _fireRate = Random.Range(3f, 7f);
            // Set the next fire time
            _canFire = Time.time + _fireRate;
            // Instantiate the enemy laser
            GameObject enemyLaser = Instantiate(_EnemylaserPrefab, transform.position, Quaternion.identity);

            // Get all Laser components in the instantiated enemy laser
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();

            // Assign each laser as an enemy laser
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }

        // Uncomment to pause the game for debugging purposes
        // Debug.Break();
    }

    void CalculateMovement()
    {
        // Move the enemy downwards
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // If the enemy moves off the bottom of the screen, reposition it at the top with a random x position
        if (transform.position.y <= -9.6f)
        {
            transform.position = new Vector3(Random.Range(-15.0f, 15.0f), 9.6f, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the enemy collides with the player
        if (other.tag == "Player")
        {
            // Get the Player component and apply damage
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            // Destroy the enemy
            DestroyEnemy();
        }

        // Check if the enemy collides with a non-enemy laser
        if (other.tag == "Laser" && !other.GetComponent<Laser>().IsEnemyLaser())
        {
            // Destroy the laser
            Destroy(other.gameObject);

            // If the player is not null, add score to the player
            if (_player != null)
            {
                _player.AddScore(1);
            }
            // Destroy the enemy
            DestroyEnemy();
        }
    }

    public void DestroyEnemy()
    {
        // Disable the enemy's collider
        GetComponent<Collider2D>().enabled = false;
        // Slow down the enemy's speed for the death animation
        _speed = 1;
        // Trigger the enemy death animation
        _animator.SetTrigger("OnEnemyDeath");
        // Play the death sound
        _audioSource.Play();
        // Destroy the enemy object after 2 seconds
        Destroy(this.gameObject, 2.0f);
    }
}
