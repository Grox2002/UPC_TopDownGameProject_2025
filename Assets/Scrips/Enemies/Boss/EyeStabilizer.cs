using UnityEngine;

public class EyeStabilizer : MonoBehaviour
{
    public Vector3 targetPosition; // El punto que queremos mirar

    void Update()
    {
        // Calcula la dirección desde el ojo hasta el target
        Vector3 direction = targetPosition - transform.position;

        // Calcula el ángulo en grados
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica la rotación al sprite
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}