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

    // ===================== MÉTODOS =====================
    void Start()
    {
        _currentHealth = _maxHealth;

        _sr = GetComponent<SpriteRenderer>();
        _playerAttack = GetComponent<P_Attack>();

        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);
    }

    // Recibe daño (ahora puede ser float)
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
            //GameManager.Instance.GameOver();
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

}
