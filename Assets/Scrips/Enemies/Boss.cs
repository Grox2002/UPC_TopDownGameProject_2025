
using System.Collections;
using UnityEngine;


public class Boss : MonoBehaviour
{
    //Variables
    [Header("Health Settings")]
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _playerBulletDamage;
    private SpriteRenderer _spriteRenderer;


    [Header("Movement Settings")]
    public Transform player;

    [Header("Attack Settings")]
    private bool _canFight;

    [Header("Balas Radiales")]
    public Transform firePoint;
    public GameObject projectilePrefab;

    [Header("Drones de persecucion")]
    public GameObject minionPrefab;

    [Header("Embestida")]
    [SerializeField] private int _dañoEmbestida;
    [SerializeField] private float _dashSpeed = 3f;
    [SerializeField] private float _dashDuration = 0.4f;
    private bool _isDashing = false;

   

    //Metodos
    void Start()
    {
        _currentHealth = _maxHealth;

        _spriteRenderer = GetComponent<SpriteRenderer>();

        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

    }

    public void StartBattle()
    {
        if (player != null && !_canFight)
        {
            _canFight = true;
            StartCoroutine(PatronAtaque());
        }
        else
        {
            StopAttack(); 
        }
    }


    public void TakeDamage(int damage)
    {

        _currentHealth -= damage;


        if (_currentHealth <= 0)
        {
            Die();
            GameManager.Instance.BossDefeated();
        }
        else
        {
            UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);
            StartCoroutine(DamageEffect());
        }
    }


    void Die()
    {
        Destroy(gameObject);
    
        StopAllCoroutines();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isDashing && collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<P_Health>().TakeDamage(_dañoEmbestida);
            Debug.Log("¡Embestida!");
        }
    }

    //Damage effect
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


    //Fases de ataque
    IEnumerator PatronAtaque()
    {
        while (_currentHealth > 0)
        {
            if ((float)_currentHealth / _maxHealth > 0.5f)
                yield return AttackPhase1();
            else
                yield return AttackPhase2();

            yield return new WaitForSeconds(1.5f);
           
        }
    }

    IEnumerator AttackPhase1()
    {
        ShootRadial(8);
        yield return new WaitForSeconds(1f);

        yield return DashTowardsPlayer();

        yield return Retreat(1.5f, 3f);

    }

    IEnumerator AttackPhase2()
    {
        // Disparo en espiral
        Debug.Log("Iniciando fase 2 del jefe");
        for (int i = 0; i < 20; i++)
        {
            ShootRadial(12, i * 10); 
            yield return new WaitForSeconds(0.2f);
        }

        // Dash doble
        yield return DashTowardsPlayer();
        yield return new WaitForSeconds(0.5f);
        yield return DashTowardsPlayer();

        yield return Retreat(1.5f, 3f);


        Instantiate(minionPrefab, transform.position + Vector3.right * 2, Quaternion.identity);
    }


    // ------------------ ATAQUES & Movimiento ------------------

    //Balas radiales-------------------------------------------------------------------------------------------------
    void ShootRadial(int bulletCount, float angleOffset = 0f)
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleStep * i + angleOffset;
            Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                                 Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

            projectile.GetComponent<FireBall>().SetDirection(shootDirection);
        }
    }

    //Embestida-------------------------------------------------------------------------------------------------
    IEnumerator DashTowardsPlayer()
    {
        if (player == null) yield break;

        _isDashing = true;

        Vector2 startPos = transform.position;
        Vector2 targetPos = player.position;
        float timer = 0f;

        while (timer < _dashDuration)
        {
            timer += Time.deltaTime;
            float time = timer / _dashDuration;
            transform.position = Vector2.Lerp(startPos, targetPos, time * _dashSpeed);
            yield return null;
        }

        _isDashing = false;
    }

    //Retroceso------------------------------------------------------------------------------------------------------
    IEnumerator Retreat(float retreatDuration = 2f, float retreatSpeed = 4f)
    {
        float timer = 0f;

        while (timer < retreatDuration)
        {
            Vector2 retreatDirection = (transform.position - player.position).normalized;
            transform.position += (Vector3)(retreatDirection * retreatSpeed * Time.deltaTime);

            timer += Time.deltaTime;
            yield return null;
        }
    }
    

    //Bala reflejada
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerBulletDamage); 
            Destroy(other.gameObject);
        }
    }

    private void StopAttack()
    {
        StopAllCoroutines();
        _canFight = false;
    }
}
