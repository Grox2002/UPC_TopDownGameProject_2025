using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    public static RoomController Instance;

    [SerializeField] private string roomID = "Room_01";
    [SerializeField] private List<GameObject> _doors;
    [SerializeField] private List<RoomSpawner> _spawners;
    [SerializeField] private AudioSource _doorSource;

    private Collider2D _entryDetector;
    private bool _fightStarted;


    private void Awake()
    {
        _entryDetector = GetComponent<Collider2D>();
        _fightStarted = false;

        if (PlayerPrefs.GetInt(roomID + "_Cleared", 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        if (PlayerPrefs.GetInt("Mission_CriptaHostages", 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }


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
        _entryDetector.enabled = false;

        CloseDoors();

        yield return new WaitForSeconds(0.5f);

        foreach (var spawner in _spawners)
            spawner.SpawnEnemies();

        yield return new WaitUntil(() => AllSpawnersCleared());

        // Guardar que esta sala ya se limpioo
        PlayerPrefs.SetInt(roomID + "_Cleared", 1);
        PlayerPrefs.Save();

        OpenDoors();
    }

    private bool AllSpawnersCleared()
    {
        foreach (var spawner in _spawners)
        {
            if (!spawner.AllEnemiesDefeated())
                return false;
        }
        return true;
    }

    private void CloseDoors()
    {
        _doorSource.Play();

        foreach (var door in _doors)
            door.SetActive(true);

        Debug.Log("Puertas cerradas, hora de la acción.");
    }

    private void OpenDoors()
    {
        _doorSource.Play();

        foreach (var door in _doors)
            door.SetActive(false);

        Debug.Log("Sala despejada, que bueno soy carajo.");
    }
}
