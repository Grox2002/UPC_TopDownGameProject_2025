using UnityEngine;
using UnityEngine.Splines;

public class EyeRotation : MonoBehaviour
{
    public float speed = 180f;

    void Update()
    {
        transform.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
