using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public GameObject collectiblePrefab;
    public List<GameObject> spawnPoints;  // List of spawn points (GameObjects)
    private List<GameObject> availableSpawnPoints;

    public int numberOfCollectibles = 5;

    void Start()
    {
        availableSpawnPoints = new List<GameObject>(spawnPoints);

        SpawnCollectibles();
    }

    void SpawnCollectibles()
    {
        int spawnCount = Mathf.Min(numberOfCollectibles, availableSpawnPoints.Count);

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableSpawnPoints.Count);

            GameObject spawnPoint = availableSpawnPoints[randomIndex];
            Vector3 spawnPosition = spawnPoint.transform.position;  // Get the position of the spawn point

            Instantiate(collectiblePrefab, spawnPosition, Quaternion.identity);

            availableSpawnPoints.RemoveAt(randomIndex);  // Remove the spawn point once it is used
        }
    }

    public void CollectItem(GameObject collectedSpawnPoint)
    {
        availableSpawnPoints.Add(collectedSpawnPoint);  // Add the spawn point back to available ones
    }
}
