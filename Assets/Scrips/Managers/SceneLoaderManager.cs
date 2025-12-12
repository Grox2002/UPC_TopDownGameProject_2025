using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance { get; private set; }


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameManager.Instance.player.SetActive(true);
        GameManager.Instance.playerMovement.enabled = true;
        GameManager.Instance.playerAttack.enabled = true;
        GameManager.Instance.shootController.SetActive(true);

        string sceneName = scene.name;

        if (GameManager.Instance.player == null)
            return;

        if (sceneName == "MainMenu" || sceneName == "VictoryScene")
        {
            GameManager.Instance.playerMovement.enabled = false;
            GameManager.Instance.playerAttack.enabled = false;
            return;
        }

        StartCoroutine(SpawnPlayer(sceneName));
    }



    public void LoadMainMenuScene()
    {
        PlayerPrefs.DeleteAll();
        SceneTransition.Instance.FadeOutToMainMenu();
    }
    
    
    public void LoadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.Instance.PlayerAlive();
        
    }
    private IEnumerator SpawnPlayer(string sceneName)
    {
 

        yield return null;

        GameManager.Instance.playerCollider.enabled = true;

        Vector3 spawnPos = Vector3.zero;
        float savedHealth = PlayerStats.Instance.maxHealth;

        // Checkpoint
        if (CheckPointManager.Instance.TryGetCheckpoint(out spawnPos, out savedHealth))
        {
            GameManager.Instance.playerTransform.position = spawnPos;
            GameManager.Instance.playerHealth.SetHealth(savedHealth);
            yield break;
        }

        // ReturnPortal
        string spawnID = PlayerPrefs.GetString("ReturnPortal", "");
        if (spawnID != "")
        {
            PlayerSpawnPoint[] points = FindObjectsByType<PlayerSpawnPoint>(FindObjectsInactive.Include, FindObjectsSortMode.None);

            foreach (var SpawnPoint in points)
            {
                if (SpawnPoint.id == spawnID)
                {
                    GameManager.Instance.playerTransform.position = SpawnPoint.transform.position;
                    yield break;
                }
            }
        }

        // FirstSP
        if (sceneName == "Crypt")
        {
            GameManager.Instance.playerTransform.position = FirstSP.Instance.transform.position;
        }
        if (sceneName == "Square")
        {
            GameManager.Instance.playerTransform.position = FirstSP.Instance.transform.position;
        }

        
    }

}
