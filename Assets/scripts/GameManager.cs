using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;  

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ball;
    [SerializeField] private int amountToSpawn;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Button startButton;  
    [SerializeField] private Button resetButton;  
    private bool isSpawning = false;  // To track if balls are spawning

    private List<GameObject> spawnedBalls = new List<GameObject>(); // List to hold spawned balls

    void Start()
    {
        // Set up button listeners
        startButton.onClick.AddListener(StartSpawning);
        resetButton.onClick.AddListener(ResetGame);
    }

    void StartSpawning()
    {
        if (!isSpawning)
        {
            isSpawning = true;
            StartCoroutine(SpawnBalls());
        }
    }

    // Coroutine to spawn balls
    IEnumerator SpawnBalls()
    {
        var totalSpawned = 0;

        while (totalSpawned < amountToSpawn)
        {
            var position = spawnPoint.position;
            var spawnedBall = Instantiate(ball, new Vector3(Random.Range(-0.05f, 0.05f), position.y, position.z), Quaternion.identity);
            spawnedBalls.Add(spawnedBall);  // Keep track of spawned balls
            yield return new WaitForSeconds(0.1f);
            totalSpawned++;
        }
    }

    // Method to reset the game (stop spawning and clear spawned balls)
    void ResetGame()
    {
        // Stop any ongoing spawning
        StopAllCoroutines();
        isSpawning = false;

        // Destroy all spawned balls
        foreach (var ball in spawnedBalls)
        {
            Destroy(ball);
        }
        spawnedBalls.Clear();  // Clear the list of spawned balls
    }
}
