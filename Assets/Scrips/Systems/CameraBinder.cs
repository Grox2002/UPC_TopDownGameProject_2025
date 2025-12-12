using Unity.Cinemachine;
using UnityEngine;

public class CameraBinder : MonoBehaviour
{
    private CinemachineCamera cam;

    void Start()
    {
        cam = GetComponent<CinemachineCamera>();

        if (GameManager.Instance != null && GameManager.Instance.playerTransform != null)
        {
            cam.Follow = GameManager.Instance.playerTransform;
            cam.LookAt = GameManager.Instance.playerTransform;
        }
    }
}
