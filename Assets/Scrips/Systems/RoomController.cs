using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<GameObject> puertas; 

    [SerializeField] private RoomSpawner spawner; 

    //private bool _playerInside;
    private bool _fightStarted;
    //private bool _roomCleared;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !_fightStarted)
        {
            //_playerInside = true;
            StartCoroutine(StartEncounter());
        }
    }

    private IEnumerator StartEncounter()
    {
        //_fightStarted = true;
        CloseDoors();

        
        yield return new WaitForSeconds(0.5f);

        spawner.SpawnEnemies();

        
        yield return new WaitUntil(() => spawner.AllEnemiesDefeated());

        OpenDoors();
        //_roomCleared = true;
    }

    private void CloseDoors()
    {
        foreach (var puerta in puertas)
        {
            puerta.SetActive(true); 
        }
        Debug.Log("Puertas cerradas, hora de laaccion.");
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
