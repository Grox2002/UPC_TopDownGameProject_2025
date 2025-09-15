using System.Linq;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform[] _spawnPoint;
    [SerializeField] private GameObject[] _enemy;
    [SerializeField] private float _spawnTime;
    [SerializeField] private bool _activeSpawn = true;

    private float _timeSinceLastSpawn;

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        _timeSinceLastSpawn += Time.deltaTime;
        if (_timeSinceLastSpawn >= _spawnTime)
        {
            _timeSinceLastSpawn = 0;
            ActivateSpawn();
        }
    }

    public void ActivateSpawn()
    {
        int enemyIndex = Random.Range(0, _enemy.Length);

        Transform spawnPoint = _spawnPoint[Random.Range(0, _spawnPoint.Length)];

        if (_activeSpawn)
        {
            Instantiate(_enemy[enemyIndex], spawnPoint.position, Quaternion.identity);
        }
    }

}
