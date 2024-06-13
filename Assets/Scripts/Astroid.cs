using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroid : MonoBehaviour
{
    [SerializeField]
    private float _rotSpeed = 19.0f;
    [SerializeField]
    private GameObject _explosion;
    [SerializeField]
    private SpawnManager _spawnManager;
    // Start is called before the first frame update
    void Start()
    {
        // Find and assign the SpawnManager component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate the object on the Z axis at a specified rotation speed
        transform.Rotate(Vector3.forward * _rotSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the collision is with a laser
        if (collision.tag == "Laser")
        {
            // Instantiate the explosion effect at the asteroid's position
            Instantiate(_explosion, transform.position, Quaternion.identity);

            // Destroy the laser that collided with the asteroid
            Destroy(collision.gameObject);

            // Notify the spawn manager to start spawning enemies or power-ups
            _spawnManager.StartSpawning();

            // Destroy the asteroid object
            Destroy(this.gameObject);
        }
    }
}
