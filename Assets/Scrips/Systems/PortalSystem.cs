using UnityEngine;

public class PortalSystem : MonoBehaviour
{
    void Start()
    {
        string portalID = PlayerPrefs.GetString("ReturnPortal", "Default");
        Transform spawn = GameObject.Find(portalID)?.transform;

        if (spawn != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawn.position;
        }
    }
}
