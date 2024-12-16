using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab; // Reference to the Coin prefab
    public int numberOfCoins = 10; // Number of coins to spawn
    public float spawnRadius = 50f; // Radius within which to spawn coins

    void Start()
    {
        SpawnCoins();
    }

    void SpawnCoins()
    {
        for (int i = 0; i < numberOfCoins; i++)
        {
            Vector3 randomPosition = new Vector3(
                Random.Range(-spawnRadius, spawnRadius),
                1f, // Adjust the height to ensure coins are above ground
                Random.Range(-spawnRadius, spawnRadius)
            );

            Instantiate(coinPrefab, randomPosition, Quaternion.identity);
        }
    }
}
