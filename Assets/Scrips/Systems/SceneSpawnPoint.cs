using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneSpawnPoint : MonoBehaviour
{
    public string sceneToLoad;
    public string entryPointID;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetString("ReturnPortal", entryPointID);
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    
}
