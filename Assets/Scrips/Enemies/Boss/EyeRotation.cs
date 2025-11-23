using UnityEngine;
using UnityEngine.Splines;

public class EyeRotation : MonoBehaviour
{
    public Transform boss;           
    public float orbitSpeed = 50f;   
    public Vector3 orbitAxis = Vector3.forward; 

    void Update()
    {
        if (boss == null) return;

        transform.RotateAround(boss.position, orbitAxis, orbitSpeed * Time.deltaTime);
    }
}
