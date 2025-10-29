using System.Collections.Generic;
using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefabs;

    
    [SerializeField] private Transform[] spawnPoints;

    
    [SerializeField] private int totalEnemies = 5;

    
    [SerializeField] private bool spawnOnce = true;

    private bool _hasSpawned = false;
    private List<GameObject> _activeEnemies = new List<GameObject>();

    
    public void SpawnEnemies()
    {
        if (_hasSpawned && spawnOnce) return; 
        _hasSpawned = true;

        _activeEnemies.Clear();

        
        for (int i = 0; i < totalEnemies; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
            _activeEnemies.Add(enemy);
        }

        Debug.Log($"Spawned {_activeEnemies.Count} enemies.");
    }

    
    public bool AllEnemiesDefeated()
    {
        
        _activeEnemies.RemoveAll(enemy => enemy == null);
        return _activeEnemies.Count == 0;
    }

    
    public void ResetSpawner()
    {
        _hasSpawned = false;
        _activeEnemies.Clear();
    }
}
