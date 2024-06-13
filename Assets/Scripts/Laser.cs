using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Laser : MonoBehaviour
{
    [SerializeField]
    private float _speed = 10.0f;
    [SerializeField]
    private bool _isEnemyLaser= false;
    private Player _player;

    // Start is called before the first frame update
    void Start()
    {
        // Find and assign the Player component
        _player = GameObject.Find("Player").GetComponent<Player>();

        // Null check for the player to ensure it is properly assigned
        if (_player == null)
        {
            Debug.LogError("The Player is NULL");
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if this laser is not an enemy laser
        if (_isEnemyLaser == false)
        {
            // Move the laser up
            MoveUp();
        }
        else
        {
            // Move the laser down
            MoveDown();
        }
    }

    void MoveUp()
    {
        // Move the laser upwards
        transform.Translate(Vector3.up * _speed * Time.deltaTime);

        // Check if the laser has moved off the top of the screen
        if (transform.position.y > 9f)
        {
            // Destroy the parent object if it exists, otherwise destroy this object
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void MoveDown()
    {
        // Move the laser downwards
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Check if the laser has moved off the bottom of the screen
        if (transform.position.y < -10f)
        {
            // Destroy the parent object if it exists, otherwise destroy this object
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    // Method to set this laser as an enemy laser
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    // Method to check if this laser is an enemy laser
    public bool IsEnemyLaser()
    {
        return _isEnemyLaser;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If this laser is an enemy laser and it hits another enemy, do nothing
        if (_isEnemyLaser && other.tag == "Enemy")
        {
            return;
        }

        // If this laser is an enemy laser and it hits the player
        if (other.tag == "Player" && _isEnemyLaser)
        {
            // Get the Player component and apply damage
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
        }

        // If this laser is not an enemy laser and it hits an enemy
        if (!_isEnemyLaser && other.tag == "Enemy")
        {
            // Get the Enemy component and destroy the enemy
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.DestroyEnemy();
            }
        }
    }
}
