using UnityEngine;
using System.Collections;

public class SacredGroundAttack : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject holyZonePrefab;
    public float delayBetweenZones = 1.5f;
    public int baseZoneCount = 3;

    // Ejecuta el ataque
    public IEnumerator Execute(Transform player, int phase)
    {
        int count = baseZoneCount + phase;

        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = player.position + new Vector3(
                Random.Range(-3f, 3f),
                Random.Range(-3f, 3f),
                0f
            );

            Instantiate(holyZonePrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(delayBetweenZones - (phase * 0.2f));
        }
    }
}
