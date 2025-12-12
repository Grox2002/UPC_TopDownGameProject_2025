using UnityEngine;
using System.Collections;

public class FallIntoAbyss : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _fallPoint;
   
    [Header("Fall Configuration")]
    [SerializeField] private float _fallDuration = 1f;
    [SerializeField] private float _fallPullForce = 5f;
    [SerializeField] private float _fallStopTime = 0.3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShrinkAndFall());
        }
        
    }

    private IEnumerator ShrinkAndFall()
    {
        GameManager.Instance.playerRb.linearVelocity = Vector2.zero;
        GameManager.Instance.playerMovement.canMove = false;

        // Atracción hacia el abismo
        if (_fallPoint != null)
        {
            Vector2 dir = ((Vector2)_fallPoint.position - GameManager.Instance.playerRb.position).normalized;
            GameManager.Instance.playerRb.linearVelocity = dir * _fallPullForce;
        }

        yield return new WaitForSeconds(_fallStopTime);

        // Achicamiento
        Vector3 startScale = GameManager.Instance.playerTransform.localScale;
        for (float time = 0; time < 1f; time += Time.deltaTime / _fallDuration)
        {
            GameManager.Instance.playerTransform.localScale = Vector3.Lerp(startScale, Vector3.zero, time);
            yield return null;
        }

        // se muere UwU
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }
}
