using UnityEngine;
using System.Collections;

public class E_Health : MonoBehaviour
{
    //Variables
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private int _playerProjectileDamage;

    private SpriteRenderer _spriteRenderer;
    private Animator _animator;

    private E_Movement _enemyMovement;

    //[SerializeField]  private Collider2D _enemyCollider;

    [Header("Sonido")]
    [SerializeField] private AudioSource _damagedSound;
    [SerializeField] private AudioSource _deathSound;
    

    //Metodos
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<E_Movement>();
    }

    public void TakeDamage(float damage)
    {
        _maxHealth -= damage;

        if(_maxHealth <= 0)
        {
            Die();
        }
        else
        {
            StartDamageEffect();
        }
    }

    public void DestroyEnemy()
    {
        if(gameObject != null)
        Destroy(gameObject);
        //StartCoroutine(DeathSoundCoroutine());
    }
    
    private void Die()
    {
        _animator.SetBool("IsDeath", true);

        _deathSound.Play();

        //_enemyCollider.enabled = false;

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.enabled = false;
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.bodyType = RigidbodyType2D.Kinematic;
        }

        _enemyMovement.enabled = false;

        
        
    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        _damagedSound.Play();
        //AudioSource.PlayClipAtPoint(_damagedSound, transform.position, _damagedSoundVolume);

        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

    }
    public void EndDamageEffect()
    {
        StopCoroutine(DamageEffect());
    }

    //bala reflejada
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerProjectileDamage); 
            Destroy(other.gameObject); 
        }
    }
}
