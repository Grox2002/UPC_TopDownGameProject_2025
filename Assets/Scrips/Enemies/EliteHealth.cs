using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EliteHealth : MonoBehaviour
{
    //Pasar a una clase heredada//


    [Header("Vida")]
    [SerializeField] private float _maxHealth = 50f;
    private float _currentHealth;

    [Header("Barra de vida")]
    [SerializeField] private Image _healthFill; 
    [SerializeField] private Vector3 _healthBarOffset = new Vector3(0, 1.5f, 0);

    [Header("Daño recibido")]
    [SerializeField] private int _playerProjectileDamage;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private E_Movement _enemyMovement;
    [SerializeField] private Collider2D _enemyCollider;

    [Header("Sonido")]
    [SerializeField] private AudioSource _damagedSound;
    [SerializeField] private AudioSource _deathSound;

    private Camera _mainCamera;

    [SerializeField] private int pointsGiven = 5;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<E_Movement>();
        _mainCamera = Camera.main;

        _currentHealth = _maxHealth;

        // Inicializar barra de vida
        if (_healthFill != null)
            _healthFill.fillAmount = 1f;
    }

    void LateUpdate()
    {
        // Actualiza posición de la barra si existe
        if (_healthFill != null)
        {
            Transform barTransform = _healthFill.transform.parent; 
            if (barTransform != null)
            {
                // Actualiza posición sobre el enemigo
                barTransform.position = transform.position + _healthBarOffset;

            }
        }
    }

    public void TakeDamage(float damage)
    {
        _currentHealth -= damage;
        _currentHealth = Mathf.Clamp(_currentHealth, 0, _maxHealth);

        if (_healthFill != null)
            _healthFill.fillAmount = _currentHealth / _maxHealth;

        if (_currentHealth <= 0)
            Die();
        else
            StartDamageEffect();
    }

    private void Die()
    {
        _animator.SetBool("IsDeath", true);
        _enemyCollider.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        _enemyMovement.enabled = false;

        _deathSound.Play();

        
        if (_healthFill != null)
            _healthFill.transform.parent.gameObject.SetActive(false);
    }

    private IEnumerator DamageEffect()
    {
        _damagedSound.Play();

        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    public void EndDamageEffect()
    {
        StopCoroutine(DamageEffect());
    }

    public void DestroyEnemy()
    {
        if (gameObject != null)
            //Destroy(gameObject);
            StartCoroutine(DestryEnemyAfterSound());
    }
    private IEnumerator DestryEnemyAfterSound()
    {
        yield return new WaitWhile(() => _deathSound.isPlaying);
        PlayerStats.Instance.AddPoints(pointsGiven);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerProjectileDamage);
            Destroy(other.gameObject);
        }
    }
}
