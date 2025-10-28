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

    [SerializeField]  private Collider2D _enemyCollider;

    [Header("Sonido")]
    [SerializeField] private AudioClip _damagedSound;
    [SerializeField] private AudioClip _deathSound;
    [SerializeField] private float _volume = 0.8f;

    private AudioSource _audioSource;

    //Metodos
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _enemyMovement = GetComponent<E_Movement>();
        _audioSource = GetComponent < AudioSource>();
    }

    public void TakeDamage(float damage)
    {
        _maxHealth -= damage;

        if(_maxHealth <= 0)
        {
            StopAllCoroutines();
            Die();
            //Die();
        }
        else
        {
            StartDamageEffect();
        }
    }

    public void DestroyEnemy()
    {

        Destroy(gameObject);
    }

    private void Die()
    {
        _animator.SetBool("IsDeath", true);

        _enemyCollider.enabled = false;

        _enemyMovement.enabled = false;

        _audioSource.PlayOneShot(_deathSound, _volume);

    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {
        _audioSource.PlayOneShot(_damagedSound, _volume);
        //AudioSource.PlayClipAtPoint(_damagedSound, transform.position, _volume);

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
