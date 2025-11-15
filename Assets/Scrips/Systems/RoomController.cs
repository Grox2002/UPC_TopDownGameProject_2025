using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<GameObject> puertas;
    [SerializeField] private List<RoomSpawner> spawners;

    private bool _fightStarted;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_fightStarted)
        {
            StartCoroutine(StartEncounter());
        }
    }

    private IEnumerator StartEncounter()
    {
        _fightStarted = true;

        CloseDoors();

        yield return new WaitForSeconds(0.5f);

        // Llamar a todos los spawners
        foreach (var spawner in spawners)
        {
            spawner.SpawnEnemies();
        }

        // Esperar hasta que todos los spawners tengan sus enemigos derrotados
        yield return new WaitUntil(() => AllSpawnersCleared());

        OpenDoors();
    }

    private bool AllSpawnersCleared()
    {
        foreach (var spawner in spawners)
        {
            if (!spawner.AllEnemiesDefeated())
                return false;
        }
        return true;
    }

    private void CloseDoors()
    {
        foreach (var puerta in puertas)
        {
            puerta.SetActive(true);
        }
        Debug.Log("Puertas cerradas, hora de la acción.");
    }

    private void OpenDoors()
    {
        foreach (var puerta in puertas)
        {
            puerta.SetActive(false);
        }
        Debug.Log("Sala despejada.");
    }
}
