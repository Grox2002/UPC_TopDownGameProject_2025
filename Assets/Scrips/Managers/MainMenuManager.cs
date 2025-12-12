using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Animator _animator;


    private void Start()
    {
        Cursor.visible = true;
    }


    public void Play()
    {
        PlayerStats.Instance.ResetAllStats();
        PlayerStats.Instance.ResetPoints();
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        StartCoroutine(StartTransition());
    }
    private IEnumerator StartTransition()
    {
        _animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Square");
    }
    public void Exit()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Application.Quit();
    }
}
