using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Powerup : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3.0f;
    [SerializeField]
    private int _powerupID; //0=Tripleshot 1=Speed 2=Shield
    [SerializeField]
    private AudioClip _clip;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move the object downwards at a speed adjusted for frame rate
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        // Check if the object has moved below the screen's lower bound
        if (transform.position.y < -9.6f)
        {
            // Destroy the object if it is out of bounds
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object collided with the player
        if (other.tag == "Player")
        {
            // Get the Player component from the collided object
            Player player = other.GetComponent<Player>();

            // Play the power-up sound at the object's position
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            // If the player component is found, activate the power-up based on its ID
            if (player != null)
            {
                switch (_powerupID)
                {
                    case 0:
                        // Activate the triple shot power-up
                        player.TripleShotActive();
                        break;
                    case 1:
                        // Activate the speed boost power-up
                        player.Speedboost();
                        break;
                    case 2:
                        // Activate the shield power-up
                        player.ShieldActive();
                        break;
                    default:
                        // Log an error if no valid power-up ID is found
                        Debug.Log("No valid powerUp ID");
                        break;
                }
            }

            // Destroy the power-up object after it has been collected by the player
            Destroy(this.gameObject);
        }
    }
}
