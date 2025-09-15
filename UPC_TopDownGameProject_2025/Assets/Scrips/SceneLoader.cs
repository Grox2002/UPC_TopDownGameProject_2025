using UnityEngine;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SceneLoaderManager.Instance.LoadScene(sceneToLoad);
        }
    }
}
