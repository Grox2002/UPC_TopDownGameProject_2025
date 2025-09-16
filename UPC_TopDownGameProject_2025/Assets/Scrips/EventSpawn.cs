using Unity.Cinemachine;
using UnityEngine;

public class EventSpawn : MonoBehaviour
{
    [SerializeField] private GameObject _spawnerA;
    [SerializeField] private GameObject _spawnerB;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Spaw activado, generando enemigos...");
            _spawnerA.SetActive(true);
            _spawnerB.SetActive(true);
        }
    }
}
