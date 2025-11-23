using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TheEndSequence : MonoBehaviour
{
    [SerializeField] private CreditsScroll creditsScroll;
    [SerializeField] private float delayBeforeCredits = 4f;
    [SerializeField] private float creditsDuration = 50f;
    [SerializeField] private float afterCreditsDelay = 2f;

    private void Start()
    {
        StartCoroutine(RunVictorySequence());
    }

    private IEnumerator RunVictorySequence()
    {
        yield return new WaitForSeconds(delayBeforeCredits);

        if (creditsScroll != null)
            creditsScroll.enabled = true;

        yield return new WaitForSeconds(creditsDuration);

        if (creditsScroll != null)
            creditsScroll.enabled = false;

        yield return new WaitForSeconds(afterCreditsDelay);

        SceneManager.LoadScene("MainMenu");
    }
}
