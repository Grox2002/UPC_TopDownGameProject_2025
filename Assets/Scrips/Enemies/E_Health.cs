using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class E_Health : MonoBehaviour
{
    //Variables
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private int _playerProjectileDamage;
    [SerializeField] private int pointsGiven = 5;

   private SpriteRenderer _spriteRenderer;
    private Animator _animator;
    private Collider2D _collider;

    private E_Movement _enemyMovement;

    

    [Header("Sonido")]
    [SerializeField] private AudioSource _damagedSound;
    [SerializeField] private AudioSource _deathSound;
    

    //Metodos
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<E_Movement>();
        _collider = GetComponent<Collider2D>();
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
        //Destroy(gameObject);
        StartCoroutine(DestryEnemyAfterSound());
    }
    private IEnumerator DestryEnemyAfterSound()
    {
        PlayerStats.Instance.AddPoints(pointsGiven);

        yield return new WaitWhile(() => _deathSound.isPlaying);
        
        Destroy(gameObject);
    }


    private void Die()
    {
        _collider.enabled = false;
        if (_animator != null)
        _animator.SetBool("IsDeath", true);

        if (_deathSound != null)
            _deathSound.Play();

        

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
        if (_enemyMovement != null)
        _enemyMovement.enabled = false;

        
        
    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        if(_damagedSound != null)
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
