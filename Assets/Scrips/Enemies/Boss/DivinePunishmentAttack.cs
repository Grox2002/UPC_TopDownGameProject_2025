using UnityEngine;
using System.Collections;

public class DivinePunishmentAttack : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject divineRayPrefab;
    public float delayBetweenRays = 1f;

    public IEnumerator Execute(Transform player, int phase)
    {
        int count = Mathf.Clamp(phase, 1, 3);

        for (int i = 0; i < count; i++)
        {
            Vector3 target = player.position;

            GameObject indicator = Instantiate(divineRayPrefab, target, Quaternion.identity);

            yield return new WaitForSeconds(delayBetweenRays - (phase * 0.1f));

   

            yield return new WaitForSeconds(0.5f);
        }
    }
}
