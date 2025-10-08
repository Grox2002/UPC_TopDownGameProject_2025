using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private GameObject[] _enemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _activeSpawn = true;
    [SerializeField] private int _maxEnemies;

    
    private float _timeSinceLastSpawn;
    private int _currentEnemy;

    private void Start()
    {
        gameObject.SetActive(false);

    }

    private void Update()
    {
        _currentEnemy = GameObject.FindGameObjectsWithTag("Enemy").Length;
        _timeSinceLastSpawn += Time.deltaTime;

        if (_timeSinceLastSpawn >= _spawnTime && _currentEnemy < _maxEnemies)
        {
            _timeSinceLastSpawn = 0;
            ActivateSpawn();
        }
    }

    public void ActivateSpawn()
    {
        int enemyType = Random.Range(0, _enemy.Length);

        Transform spawnPoint = _spawnPoint[Random.Range(0, _spawnPoint.Length)];

        if (_activeSpawn)
        {
            Instantiate(_enemy[enemyType], spawnPoint.position, Quaternion.identity);
        }
    }
}
