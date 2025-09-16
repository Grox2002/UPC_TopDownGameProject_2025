using UnityEngine;
using System.Collections;

public class E_Health : MonoBehaviour
{
    //Variables
    [SerializeField] private float _maxHealth = 50f;
    [SerializeField] private int _playerProjectileDamage;

    private SpriteRenderer _spriteRenderer;
   

    //Metodos
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
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

    public void Die()
    {
        Destroy(gameObject);
    }

    public void StartDamageEffect()
    {
        StartCoroutine(DamageEffect());
    }

    private IEnumerator DamageEffect()
    {

        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.enabled = true;
            yield return new WaitForSeconds(0.1f);
        }

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
