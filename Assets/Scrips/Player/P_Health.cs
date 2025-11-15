using System.Collections;
using UnityEngine;


public class P_Health : MonoBehaviour
{
    // ===================== VARIABLES =====================
    [Header("Salud")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _currentHealth;

    private SpriteRenderer _sr;
    private bool _isInvulnerable;
    private P_Attack _playerAttack;

    [SerializeField] private Transform fallPoint; // Asigna en el inspector
    [SerializeField] private float fallPullForce = 5f; // fuerza inicial hacia el punto
    [SerializeField] private float fallStopTime = 0.3f; // tiempo antes de empezar a achicar

    // ===================== MÉTODOS =====================
    void Start()
    {
        _currentHealth = _maxHealth;

        _sr = GetComponent<SpriteRenderer>();
        _playerAttack = GetComponent<P_Attack>();

        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);
    }

    // Recibe daño
    public void TakeDamage(float damage)
    {
        if (_isInvulnerable && damage > 0f) return;

        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, _maxHealth);
        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);

        if (_currentHealth <= 0f)
        {
            GameManager.Instance.GameOver();
            Die();
            
        }
        else if (damage > 0f)
        {
            StartCoroutine(DamageEffect());
        }
    }

    public void Die()
    {
        if (_playerAttack != null)
            _playerAttack.enabled = false;

        Destroy(gameObject);
        // o: gameObject.SetActive(false);
    }

    private IEnumerator DamageEffect()
    {
        _isInvulnerable = true;

        for (int i = 0; i < 6; i++)
        {
            _sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        _isInvulnerable = false;
    }

    public void Heal(float amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);
        Debug.Log($"Has restaurado {amount} de vida");
    }

    private IEnumerator ShrinkAndFall()
    {
        float duration = 1f;
        float elapsed = 0f;

        Vector3 initialScale = transform.localScale;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Desactiva el movimiento del jugador
        if (TryGetComponent<P_Movement>(out var movement))
            movement.enabled = false;

        // Atraccion hacia el centro del abismo
        if (fallPoint != null)
        {
            Vector2 dir = ((Vector2)fallPoint.position - rb.position).normalized;
            rb.linearVelocity = dir * fallPullForce;
        }

        yield return new WaitForSeconds(fallStopTime);

        // Detiene el movimiento
        rb.linearVelocity = Vector2.zero;

        // Efecto de achicamiento
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, t);
            yield return null;
        }

        // Final: Game Over + destrucción
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }

}
