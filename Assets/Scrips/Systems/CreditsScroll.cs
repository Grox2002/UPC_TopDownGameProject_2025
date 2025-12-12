using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsScroll : MonoBehaviour
{
    [SerializeField] private float speed = 50f;
    [SerializeField] private GameObject objectToScroll;

    private bool _canScroll = false; 

    void Start()
    {
        StartCoroutine(CreditsCoroutine());
    }

    void Update()
    {
        if (_canScroll)
        {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
        }
    }

    private IEnumerator CreditsCoroutine()
    {
        yield return new WaitForSeconds(3f);
        _canScroll = true; 

        yield return new WaitForSeconds(27f);
        _canScroll = false; 

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }
}
