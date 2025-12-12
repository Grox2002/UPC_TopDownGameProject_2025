using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class PortalSystem : MonoBehaviour
{
    public string sceneToLoad;
    public string entryPointID;

    [SerializeField] private SceneTransition _transitionScript;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            StartCoroutine(LoadSceneRoutine());
    }

    private IEnumerator LoadSceneRoutine()
    {
        // Marcar el portal al que se debe volver
        PlayerPrefs.SetString("ReturnPortal", entryPointID);

        // Efecto de transición
        if (_transitionScript != null)
        {
            _transitionScript.PlayFadeOut();
            yield return new WaitForSeconds(0.5f);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}
