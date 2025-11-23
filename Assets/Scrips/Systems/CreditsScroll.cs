using UnityEngine;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    //public bool canScroll = false;
    [SerializeField] private GameObject objectToScroll;
    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

}
