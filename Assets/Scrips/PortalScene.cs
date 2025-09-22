using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalScene : MonoBehaviour
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
