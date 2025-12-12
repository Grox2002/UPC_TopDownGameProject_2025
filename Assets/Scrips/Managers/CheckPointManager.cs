using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance;

    private Vector3 lastCheckpointPosition;
    private bool hasCheckpoint = false;
    public bool HasCheckpoint => hasCheckpoint;
    private float lastCheckpointHealth;
    private string checkpointSceneName;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

    }

    public void SetCheckpoint(Vector3 position, float health)
    {
        lastCheckpointPosition = position;
        lastCheckpointHealth = health;
        hasCheckpoint = true;

        checkpointSceneName = SceneManager.GetActiveScene().name;
    }

    public bool TryGetCheckpoint(out Vector3 pos, out float health)
    {
        if (checkpointSceneName != SceneManager.GetActiveScene().name)
        {
            pos = Vector3.zero;
            health = 0;
            return false;
        }

        pos = lastCheckpointPosition;
        health = lastCheckpointHealth;
        return hasCheckpoint;
    }
}
