using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance { get; private set;}

    private Animator _animator;

    [SerializeField] private AnimationClip _transitionClip;


    private void Awake()
    {
        Instance = this;

        _animator = GetComponentInChildren<Animator>();
    }


    public void PlayFadeOut()
    {
        _animator.SetTrigger("FadeOut");
    }


    public void FadeOutToMainMenu()
    {
        StartCoroutine(TransitionToMainMenu());
    }

    private IEnumerator TransitionToMainMenu()
    {
        _animator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("MainMenu");
    }
}

