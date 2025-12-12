using UnityEngine;

public class Targeting : MonoBehaviour
{
    [SerializeField] private float followSpeed = 2f;

    private Transform player;


    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * followSpeed * Time.deltaTime;
    }
}
