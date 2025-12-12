using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class P_Health : MonoBehaviour
{
    // ===================== VARIABLES =====================

    public static event Action<float, float> OnHealthChanged;

    public float _currentHealth;

    //cOMPONENTS
    [SerializeField] private GameObject _bow;
    private SpriteRenderer _sr;
    private P_Attack _playerAttack;
    private P_Movement _playerMovement;
    private Collider2D _collider;
    private Rigidbody2D _rb;
    private Animator _animator;

    

    [Header("Audio")]
    [SerializeField] private AudioSource _moanSource;

    private bool _isInvulnerable;


    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _playerAttack = GetComponent<P_Attack>();
        _playerMovement = GetComponent<P_Movement>();
        _collider = GetComponent<Collider2D>();
        _animator = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        _currentHealth = PlayerStats.Instance.maxHealth;

        GameManager.Instance.RegisterPlayer(gameObject);
        GameManager.Instance.PlayerAlive();

        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);
    }

    private void OnEnable()
    {
        _isInvulnerable = false;
        _currentHealth = PlayerStats.Instance.maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);

        if (SceneManager.GetActiveScene().name == "MainMenu" ||
        SceneManager.GetActiveScene().name == "VictoryScene")
        {
            _playerAttack.enabled = false;
            _playerMovement.enabled = false;
        }
    }

    //======================== DAMAGE / HEAL ===============================//
    public void TakeDamage(float amount)
    {
        if (_isInvulnerable || amount <= 0f)
            return;

        _currentHealth -= amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, PlayerStats.Instance.maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);

        if (_currentHealth <= 0f)
        {
            GameManager.Instance.PlayerDied();
            Die();
        }
        else
        {
            GameManager.Instance.PlayerAlive();
            StartCoroutine(DamageEffect());
        }
    }

    public void Heal(float amount)
    {
        if (amount <= 0f)
            return;

        _currentHealth += amount;
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, PlayerStats.Instance.maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);

        Debug.Log($"Has restaurado {amount} de vida");
    }

    // Se llama cuando aumenta el máximo de vida desde PlayerStats.
    public void RefreshHealthStats()
    {
        _currentHealth = Mathf.Clamp(_currentHealth, 0f, PlayerStats.Instance.maxHealth);

        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);
    }


    
    //===================== DEATH========================
    private void Die()
    {
        _isInvulnerable = true;
        _bow.SetActive(false);
        _collider.enabled = false;
        _rb.linearVelocity = Vector2.zero;
        _playerAttack.enabled = false;
        _playerMovement.enabled = false;

        StartCoroutine(DeathAfterAnimation());
    }

    public void SetHealth(float amount)
    {
        _currentHealth = Mathf.Clamp(amount, 0, PlayerStats.Instance.maxHealth);
        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);
    }
    public void ResetHealthToFull()
    {
        _currentHealth = PlayerStats.Instance.maxHealth;
        OnHealthChanged?.Invoke(_currentHealth, PlayerStats.Instance.maxHealth);
    }


    private IEnumerator DeathAfterAnimation()
    {
        _animator.SetTrigger("Death");
        yield return new WaitForSeconds(2f);

        gameObject.SetActive(false);
        GameManager.Instance.GameOver();
    }


    
    // DAMAGE EFFECT
    private IEnumerator DamageEffect()
    {
        _isInvulnerable = true;

        _moanSource.Play();

        for (int i = 0; i < 6; i++)
        {
            _sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _sr.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

        _isInvulnerable = false;
    }

    public float GetCurrentHealth()
    {
        return _currentHealth;
    }

}
