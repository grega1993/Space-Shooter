using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject[] _PowerUp;
    [SerializeField]
    private GameObject _enemyContainer;
    private bool _stopSpawning = false;
    private IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code can go here if needed
    }

    public void StartSpawning()
    {
        // Start the coroutine to spawn enemies
        coroutine = SpawnEnemyRoutine();
        StartCoroutine(coroutine);

        // Start the coroutine to spawn power-ups
        StartCoroutine(SpawnPowerupRoutine());
    }

    // Update is called once per frame
    void Update()
    {
        // Code that needs to run every frame can go here if needed
    }

    // Coroutine to spawn enemies
    IEnumerator SpawnEnemyRoutine()
    {
        // Wait for 3 seconds before starting the spawning process
        yield return new WaitForSeconds(3.0f);

        // Continue spawning enemies until _stopSpawning is true
        while (_stopSpawning == false)
        {
            // Generate a random spawn position within the specified range
            Vector3 spawnPos = new Vector3(Random.Range(-15.0f, 15.0f), 9.6f, 0);

            // Instantiate a new enemy at the generated position
            GameObject newEnemy = Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);

            // Set the new enemy's parent to the enemy container
            newEnemy.transform.parent = _enemyContainer.transform;

            // Wait for 1 second before spawning the next enemy
            yield return new WaitForSeconds(2.0f);
        }

        // Destroy the enemy container when spawning is stopped
        Destroy(_enemyContainer);
    }

    // Coroutine to spawn power-ups
    IEnumerator SpawnPowerupRoutine()
    {
        // Wait for 3 seconds before starting the spawning process
        yield return new WaitForSeconds(3.0f);

        // Continue spawning power-ups until _stopSpawning is true
        while (_stopSpawning == false)
        {
            // Wait for a random time between 3 and 7 seconds before spawning the next power-up
            yield return new WaitForSeconds(Random.Range(3.0f, 7.0f));

            // Generate a random spawn position within the specified range
            Vector3 spawnPos = new Vector3(Random.Range(-15.0f, 15.0f), 9.6f, 0);

            // Generate a random index to select a power-up from the array
            int randomPowerUp = Random.Range(0, 3); // Random value from 0 to 2

            // Instantiate the selected power-up at the generated position
            GameObject newPowerup = Instantiate(_PowerUp[randomPowerUp], spawnPos, Quaternion.identity);

            // Power-up spawning happens every 3 to 7 seconds
        }
    }

    // Method to stop spawning when the player dies
    public void onPlayerDeath()
    {
        _stopSpawning = true;
    }
}
