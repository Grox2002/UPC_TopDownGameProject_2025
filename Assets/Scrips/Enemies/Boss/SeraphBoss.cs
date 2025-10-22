using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphBoss : MonoBehaviour
{
    //===============================VARIABLES====================================================//

    [Header("Configuración de salud")]
    [SerializeField] private int _maxHealth = 1000;
    private int _currentHealth;
    private int _currentPhase = 1;

    [Header("Referencias")]
    [SerializeField] private Transform player;
    private SpriteRenderer _spriteRenderer;
    private bool _canFight;

    [Header("Ataque: Suelo Sagrado")]
    public GameObject sacredZonePrefab;
    public Transform[] spawnPoints;       
    public int zonesPerWave = 3;

    [SerializeField] private float attackInterval = 5f;

    [Header("Ataque: Catigo divino")]
    public GameObject sacredLightningPrefab;

    [Header("Ataque: Embestida")]
    [SerializeField] private float riseHeight = 2f;
    [SerializeField] private float riseSpeed = 3f;
    [SerializeField] private float dashDistance = 6f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float retreatDistance = 3f;
    [SerializeField] private float retreatSpeed = 5f;
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float pauseBeforeDash = 0.4f;
    [SerializeField] private float damage = 40f;
    public GameObject dashDamager;
    public LayerMask playerLayer;
    private bool _isAttacking = false;
    private Vector2 _initialPosition;


    [Header("Ataque: Absorcion")]
    public GameObject orbPrefab;
    public float pullForce = 5f;
    public int baseOrbCount = 4;
    public float orbCooldown = 2f;
    public Transform firePoint;

    [Header("Efecto de flote")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2f;
    private float _floatOffset;

    //===============================METODOS====================================================//

    private void Start()
    {
        _currentHealth = _maxHealth;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        _floatOffset = Random.Range(0f, Mathf.PI * 2f);
        StartCoroutine(FloatEffect());

        _initialPosition = transform.position;

        StartBattle();
    }

    public void StartBattle()
    {
        if (player != null && !_canFight)
        {
            _canFight = true;
            StartCoroutine(PatronAtaque());
        }
    }

    private IEnumerator PatronAtaque()
    {
        List<IEnumerator> availableAttacks = new List<IEnumerator>();

        while (_currentHealth > 0)
        {
            yield return new WaitForSeconds(attackInterval);

            // ===== Determina la fase actual ===== //
            int newPhase;
            if (_currentHealth > _maxHealth * 0.66f)
                newPhase = 1;
            else if (_currentHealth > _maxHealth * 0.33f)
                newPhase = 2;
            else
                newPhase = 3;

            // ===== Cada cambio de fase, activa absorción ===== //
            if (newPhase != _currentPhase)
            {
                Debug.Log($"Cambiando a fase {newPhase}, ejecutando absorción...");
                _currentPhase = newPhase;
                
                yield return StartCoroutine(AbsorptionAttack(_currentPhase));

                yield return new WaitForSeconds(1f);
            }

            // ===== Ataques según la fase ===== //
            availableAttacks.Clear();

            if (_currentPhase == 1) //Fase 1
            {
                availableAttacks.Add(CelestialCharge());
            }
            else if (_currentPhase == 2) // Fase 2
            {
                availableAttacks.Add(CelestialCharge());
                availableAttacks.Add(SpawnSacredZonesCoroutine());
            }
            else // Fase 3
            {
                availableAttacks.Add(CelestialCharge());
                availableAttacks.Add(SpawnSacredZonesCoroutine());
                availableAttacks.Add(DivinePunishmentCoroutine());
            }

            // ===== Elegir ataque aleatorio =====
            IEnumerator selectedAttack = availableAttacks[Random.Range(0, availableAttacks.Count)];
            StartCoroutine(selectedAttack);
        }
    }

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            Destroy(gameObject);
            GameManager.Instance.BossDefeated();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        _spriteRenderer.enabled = true;
    }



    private IEnumerator SpawnSacredZonesCoroutine()
    {
        int totalZones = Mathf.Min(zonesPerWave, spawnPoints.Length);

        for (int i = 0; i < totalZones; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(sacredZonePrefab, spawnPoint.position, Quaternion.identity);
        }

        yield return new WaitForSeconds(0.2f);
    }

    private IEnumerator DivinePunishmentCoroutine()
    {
        if (player == null) yield break;
       
        yield return new WaitForSeconds(0.5f);

        GameObject lightning = Instantiate(sacredLightningPrefab, player.position, Quaternion.identity);

        //DivinePunishmentAttack slf = lightning.GetComponent<DivinePunishmentAttack>();

        yield return null;
    }

    public IEnumerator CelestialCharge()
    {

        if (player == null || _isAttacking) yield break;
        _isAttacking = true;

        Vector2 startPos = transform.position;
        Vector2 riseTarget = startPos + Vector2.up * riseHeight;

        // Elevación del boss
        while (Vector2.Distance(transform.position, riseTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, riseTarget, riseSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(pauseBeforeDash);

        dashDamager.SetActive(true);

        // Embestida hacia el jugador.
        Vector2 dashDir = (player.position - transform.position).normalized;
        Vector2 dashTarget = (Vector2)transform.position + dashDir * dashDistance;
        
        while (Vector2.Distance(transform.position, dashTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);

            yield return null;
        }
        
        dashDamager.SetActive(false);

        // Retroceso
        Vector2 retreatDir = -dashDir;
        Vector2 retreatTarget = (Vector2)transform.position + retreatDir * retreatDistance;

        while (Vector2.Distance(transform.position, retreatTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, retreatTarget, retreatSpeed * Time.deltaTime);
            yield return null;
        }

        // Retorno a la posición inicial
        while (Vector2.Distance(transform.position, _initialPosition) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        _isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }

    public IEnumerator AbsorptionAttack(int phase)
    {
        Debug.Log($"Iniciando ataque de absorción (fase {phase})");

        float absorptionDuration = 2f + (phase * 0.5f); 
        float totalDuration = 5f;                       
        float shootInterval = 0.25f; 
        float shootTimer = 0f;
        float elapsed = 0f;
        float absorptionTimer = 0f;
        float angleOffset = 0f;

        int bulletCount = 12; // cantidad de balas por anillo

        // --- Mientras dure todo el ataque ---
        while (elapsed < totalDuration)
        {
            // --- 1. Atraer jugador sólo durante absorptionDuration ---
            if (absorptionTimer < absorptionDuration && player != null)
            {
                Vector3 dir = (transform.position - player.position).normalized;
                player.position += dir * pullForce * Time.deltaTime;
                absorptionTimer += Time.deltaTime;
            }

            // --- 2. Disparo continuo en espiral ---
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootRadial(bulletCount, angleOffset);
                angleOffset += 10f; // hace girar el patrón
                shootTimer = 0f;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(orbCooldown);
        Debug.Log("Absorción finalizada");
    }

    void ShootRadial(int bulletCount, float angleOffset = 0f)
    {
        float angleStep = 360f / bulletCount;

        for (int i = 0; i < bulletCount; i++)
        {
            float angle = angleStep * i + angleOffset;
            Vector2 shootDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),
                                                 Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject projectile = Instantiate(orbPrefab, firePoint.position, Quaternion.identity);

            projectile.GetComponent<FireBall>().SetDirection(shootDirection);
        }
    }

    private IEnumerator FloatEffect()
    {
        Vector3 basePos = visualTransform.localPosition;

        while (true)
        {
            if (!_isAttacking && visualTransform != null)
            {
                float newY = Mathf.Sin(Time.time * floatSpeed + _floatOffset) * floatAmplitude;
                visualTransform.localPosition = new Vector3(basePos.x, basePos.y + newY, basePos.z);
            }
            else if (visualTransform != null)
            {
                visualTransform.localPosition = basePos;
            }

            yield return null;
        }
    }
}




