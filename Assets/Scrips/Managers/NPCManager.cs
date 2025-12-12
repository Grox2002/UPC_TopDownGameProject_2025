using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public GameObject npcInicio;
    public GameObject npcCripta;

    void Start()
    {
       bool completed = MissionManager.Instance.CryptaHostagesCompleted;

        npcInicio.SetActive(!completed);
        npcCripta.SetActive(completed);
    }
}
