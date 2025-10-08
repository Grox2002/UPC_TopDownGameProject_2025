using UnityEngine;
using System.Collections;

public class AbsorptionAttack : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject orbPrefab;
    public float pullForce = 5f;
    public int baseOrbCount = 4;
    public float orbCooldown = 2f;

    public IEnumerator Execute(Transform boss, Transform player, int phase)
    {
        // Atracción del jugador
        float duration = 1.5f + (phase * 0.5f);
        float timer = 0f;

        while (timer < duration)
        {
            Vector3 dir = (boss.position - player.position).normalized;
            player.position += dir * pullForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        // Spawn de orbes alrededor del boss
        int orbs = baseOrbCount + phase * 2;
        for (int i = 0; i < orbs; i++)
        {
            float angle = i * (360f / orbs);
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Instantiate(orbPrefab, boss.position, rot);
        }

        yield return new WaitForSeconds(orbCooldown);
    }
}
