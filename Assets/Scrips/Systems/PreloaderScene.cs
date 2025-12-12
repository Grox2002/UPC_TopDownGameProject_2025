using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PreloaderScene : MonoBehaviour
{
    [Header("Scene to load")]
    public string sceneToLoad;

    private AsyncOperation preloadOperation;
    private bool isPreloaded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isPreloaded)
        {
            StartCoroutine(PreloadScene());
        }
    }

    private IEnumerator PreloadScene()
    {
        isPreloaded = true;

        // Comienza a cargar la escena en segundo plano
        preloadOperation = SceneManager.LoadSceneAsync(sceneToLoad);
        preloadOperation.allowSceneActivation = false;

        while (preloadOperation.progress < 0.9f)
        {
            yield return null;
        }

        Debug.Log("Escena precargada: " + sceneToLoad);
    }
     
    //Esto lo llamo desde PortalSystem para cargar la escena
    public void ActivateScene()
    {
        if (preloadOperation != null)
        {
            preloadOperation.allowSceneActivation = true;
        }
    }
}
