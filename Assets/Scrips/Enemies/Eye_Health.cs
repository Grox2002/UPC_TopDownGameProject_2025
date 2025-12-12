using UnityEngine;
using System.Collections;

public class Eye_Health : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 20f;
    [SerializeField] private int _playerProjectileDamage;
    [SerializeField] private GameObject _healOrbPrefab;
    [SerializeField] private AnimationClip _deathClip;

    private SpriteRenderer _spriteRenderer;
    private SeraphBoss _boss;
    private Animator _animator;


    [Header("Sonido")]
    [SerializeField] private AudioSource _damagedSound;


    //Metodos
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _boss = GetComponentInParent<SeraphBoss>();
    }

    public void TakeDamage(float damage)
    {
        _maxHealth -= damage;

        if (_maxHealth <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private void Die()
    {
       StartCoroutine(DeactivateAfterDelay());

       if (_healOrbPrefab != null)
           SpawnHealOrb();
    }

    private void SpawnHealOrb()
    {
        Instantiate(_healOrbPrefab, transform.position, Quaternion.identity);
    }


    private IEnumerator DeactivateAfterDelay()
    {
        _damagedSound.Play();
        _animator.SetTrigger("StartDeath");

        yield return new WaitForSeconds(0.7f);

        gameObject.SetActive(false);
        _boss.CheckIfAllEyesInactive();
    }

    private IEnumerator DamageEffect()
    {
        if (_damagedSound != null)
            _damagedSound.Play();

        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

    }

}
