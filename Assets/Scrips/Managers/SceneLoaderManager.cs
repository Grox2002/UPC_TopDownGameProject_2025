using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderManager : MonoBehaviour
{
    public static SceneLoaderManager Instance { get; private set; }

    [SerializeField] private CreditsScroll _creditsScroll;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName);
    }

    //Victory scene
    public void LoadVictoryScene()
    {
        StartCoroutine(LoadSceneAfterDelay());
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        SceneManager.LoadScene("VictoryScene");
        yield return new WaitForSeconds(4f);
        _creditsScroll.enabled = true;
        yield return new WaitForSeconds(8f);
        _creditsScroll.enabled = false;
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

    
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("MainMenu");
    }

    
    public void LoadActiveScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}
