using System.Collections;
using UnityEngine;


public class P_Health : MonoBehaviour
{
    //Variables
    [SerializeField] private int _maxHealth = 100;
    [SerializeField] private int _currentHealth;

    private SpriteRenderer _sr;
    private bool _isInvulnerable;
    private P_Attack _playerAttack;

    //Metodos
    void Start()
    {
        _currentHealth = _maxHealth;

        _sr = GetComponent<SpriteRenderer>();

        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);

        _playerAttack = GetComponent<P_Attack>();
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            GameManager.Instance.GameOver();
            Die();
        }
        else
        {
            UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);

            if (_isInvulnerable) return;

            _isInvulnerable = true;
            StartCoroutine(DamageEffect());
        }
    }


    public void Die()
    {
        Destroy(gameObject);
        _playerAttack.enabled = false;
        //gameObject.SetActive(false);
    }


    //Damage effect
    private IEnumerator DamageEffect()//(int numFlashes = 3, float flashDuration = 0.1f)
    {
        for (int i = 0; i < 6; i++)
        {
            _sr.enabled = false;
            yield return new WaitForSeconds(0.1f);
            _sr.enabled = true;
            yield return new WaitForSeconds(0.1f);

            _isInvulnerable = false;
        }

        /*
        Color originalColor = _sr.color;
        float originalAlpha = originalColor.a;

        for (int i = 0; i < numFlashes; i++)
        {
            // solo cambio el alpha
            _sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0.3f);
            yield return new WaitForSeconds(flashDuration);

            _sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalAlpha);
            yield return new WaitForSeconds(flashDuration);
        }

  
        _sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, originalAlpha);
        */
    }


    public void Heal(int amount)
    {
        _currentHealth = Mathf.Min(_currentHealth + amount, _maxHealth);
        Debug.Log($"Has restaurado: [amount] de vida");
        UI_Manager.Instance.UpdatePlayerHealthBar(_currentHealth, _maxHealth);
    }

}
