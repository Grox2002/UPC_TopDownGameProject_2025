using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeraphBoss : MonoBehaviour
{
    //===============================VARIABLES====================================================//

    [Header("Health Configuration")]
    [SerializeField] private int _maxHealth = 1000;
    private int _currentHealth;
    private int _currentPhase = 1;
    private bool _isInvulnerable = false;

    [Header("References")]
    [SerializeField] private Transform player;
    private SpriteRenderer _spriteRenderer;
    private bool _canFight;
    private AudioSource _audioSource;

    [Header("Attack: Sacred Ground")]
    public GameObject sacredZonePrefab;
    public Transform[] spawnPoints;
    public int zonesPerWave = 3;

    [SerializeField] private float attackInterval = 5f;

    [Header("Attack: Divine Punishment")]
    public GameObject sacredLightningPrefab;
    [SerializeField] private AudioClip _laserImpactSound;

    [Header("Attack: Celestial Charge")]
    [SerializeField] private float riseHeight = 2f;
    [SerializeField] private float riseSpeed = 3f;
    [SerializeField] private float dashDistance = 6f;
    [SerializeField] private float dashSpeed = 12f;
    [SerializeField] private float retreatDistance = 3f;
    [SerializeField] private float retreatSpeed = 5f;
    [SerializeField] private float returnSpeed = 4f;
    [SerializeField] private float pauseBeforeDash = 0.4f;
    public GameObject dashDamager;
    public LayerMask playerLayer;
    private bool _isAttacking = false;
    private Vector2 _initialPosition;

    [Header("Ataque: Radial Fire")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float pullForce = 5f;
    [SerializeField] private int baseOrbCount = 4;
    [SerializeField] private float orbCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private AudioClip _fireBallSound;
    [SerializeField] private float _volume;

    [Header("Float Effect")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2f;
    private float _floatOffset;

    private bool _isTransitioning = false;

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
        _audioSource = GetComponent<AudioSource>();
    }

    public void StartBattle()
    {
        if (player != null && !_canFight)
        {
            _canFight = true;
            _currentAttackRoutine = StartCoroutine(PatronAtaque());
        }
    }

    private Coroutine _currentAttackRoutine;
    private IEnumerator PatronAtaque()
    {
        while (_currentHealth > 0)
        {
            // Espera entre ataques
            yield return new WaitForSeconds(attackInterval);

            // ===== Esperar mientras dure la transición =====
            while (_isTransitioning)
                yield return null;

            // ===== Determinar fase actual =====
            int newPhase;
            if (_currentHealth > _maxHealth * 0.66f)
                newPhase = 1;
            else if (_currentHealth > _maxHealth * 0.33f)
                newPhase = 2;
            else
                newPhase = 3;

            // ===== Cambiar de fase si es necesario =====
            if (newPhase != _currentPhase)
            {
                StartCoroutine(PhaseTransition(newPhase));
                continue; // espera en la siguiente iteración hasta que _isTransitioning sea false
            }

            // ===== Definir ataques disponibles según fase =====
            List<System.Func<IEnumerator>> availableAttacks = new List<System.Func<IEnumerator>>();
            if (_currentPhase == 1)
                availableAttacks.Add(() => CelestialCharge());
            else if (_currentPhase == 2)
            {
                availableAttacks.Add(() => CelestialCharge());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
            }
            else
            {
                availableAttacks.Add(() => DivinePunishmentCoroutine());
                availableAttacks.Add(() => CelestialCharge());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
            }

            // ===== Elegir ataque aleatorio =====
            var selectedAttackFunc = availableAttacks[Random.Range(0, availableAttacks.Count)];
            _currentAttackRoutine = StartCoroutine(selectedAttackFunc());
            yield return _currentAttackRoutine;
            _currentAttackRoutine = null;
        }
    }

    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return; // Invulnerable durante transición de fase

        _currentHealth -= damage;
        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            UI_Manager.Instance.HideBossHealthBar();
            Die();
            GameManager.Instance.BossDefeated();
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
    }

    private void Die()
    {
        //UI_Manager.Instance.HideBossHealthBar();
        Destroy(gameObject);
        //GameManager.Instance.BossDefeated();
    }
    private IEnumerator DamageEffect()
    {
        Color originalColor = _spriteRenderer.color;
        Color damageColor = new Color(1f, 1f, 1f, 0.5f); 

        for (int i = 0; i < 2; i++)
        {
            _spriteRenderer.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
    /*
    private IEnumerator DamageEffect()
    {
        for (int i = 0; i < 6; i++)
        {
            _spriteRenderer.enabled = !_spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
        }
        _spriteRenderer.enabled = true;
    }
    */
    private IEnumerator PhaseTransition(int newPhase)
    {
        _isTransitioning = true;
        _isInvulnerable = true;
        _spriteRenderer.color = Color.yellow;

        Debug.Log($"Cambiando a fase {newPhase}...");

        
        yield return StartCoroutine(RadialAttack(newPhase));

        
        yield return new WaitForSeconds(1f);

        _spriteRenderer.color = Color.white;
        _isInvulnerable = false;
        _isTransitioning = false;

        _currentPhase = newPhase;
        Debug.Log($"Fase {newPhase} completada");
    }

    private IEnumerator SpawnSacredZonesCoroutine()
    {
        int totalZones = Mathf.Min(zonesPerWave, spawnPoints.Length);
        List<GameObject> activeZones = new List<GameObject>();

        // Instancia las zonas
        for (int i = 0; i < totalZones; i++)
        {
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject zone = Instantiate(sacredZonePrefab, spawnPoint.position, Quaternion.identity);
            activeZones.Add(zone);
        }

        // Duración del ataque
        float zoneDuration = 4f;
        yield return new WaitForSeconds(zoneDuration);

        // Limpieza de zonas
        foreach (GameObject zone in activeZones)
        {
            if (zone != null)
                Destroy(zone);
        }

        yield return null;
    }

    private IEnumerator DivinePunishmentCoroutine()
    {
        _audioSource.PlayOneShot(_laserImpactSound, _volume);

        if (player == null) yield break;
        yield return new WaitForSeconds(0.5f);
        Instantiate(sacredLightningPrefab, player.position, Quaternion.identity);
        yield return null;
    }

    public IEnumerator CelestialCharge()
    {
        if (player == null || _isAttacking) yield break;
        _isAttacking = true;

        Vector2 startPos = transform.position;
        Vector2 riseTarget = startPos + Vector2.up * riseHeight;

        while (Vector2.Distance(transform.position, riseTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, riseTarget, riseSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(pauseBeforeDash);

        dashDamager.SetActive(true);

        Vector2 dashDir = (player.position - transform.position).normalized;
        Vector2 dashTarget = (Vector2)transform.position + dashDir * dashDistance;

        while (Vector2.Distance(transform.position, dashTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, dashTarget, dashSpeed * Time.deltaTime);
            yield return null;
        }

        dashDamager.SetActive(false);

        Vector2 retreatDir = -dashDir;
        Vector2 retreatTarget = (Vector2)transform.position + retreatDir * retreatDistance;

        while (Vector2.Distance(transform.position, retreatTarget) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, retreatTarget, retreatSpeed * Time.deltaTime);
            yield return null;
        }

        while (Vector2.Distance(transform.position, _initialPosition) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, _initialPosition, returnSpeed * Time.deltaTime);
            yield return null;
        }

        _isAttacking = false;
    }

    public IEnumerator RadialAttack(int phase)
    {
        Debug.Log($"Iniciando ataque radial (fase {phase})");

        float totalDuration = 5f;        
        float shootInterval = 0.25f;       
        float shootTimer = 0f;
        float elapsed = 0f;
        float angleOffset = 0f;
        int bulletCount = 12;

        _audioSource.PlayOneShot(_fireBallSound, _volume);

        while (elapsed < totalDuration)
        {
            shootTimer += Time.deltaTime;
            if (shootTimer >= shootInterval)
            {
                ShootRadial(bulletCount, angleOffset);
                angleOffset += 10f;          
                shootTimer = 0f;
            }

            elapsed += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(orbCooldown);
        Debug.Log("Ataque radial finalizado");
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
            projectile.GetComponent<E_Projectile>().SetDirection(shootDirection);
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

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 0.6f);
    }
}




