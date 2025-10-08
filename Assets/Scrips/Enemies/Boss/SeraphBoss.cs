using UnityEngine;
using System.Collections;

public class SeraphBoss : MonoBehaviour
{
    // ==================== CONFIGURACIÓN GENERAL ====================
    [Header("Salud")]
    [SerializeField] private int _maxHealth = 1000;
    [SerializeField] private int _currentHealth;
    [SerializeField] private int _playerBulletDamage = 10;
    private SpriteRenderer _spriteRenderer;

    [Header("Referencias")]
    public Transform player;
    public Transform firePoint;

    [Header("Estado")]
    private bool _canFight;

    // ==================== ATAQUES ====================

    [Header("Suelo Sagrado")]
    public GameObject holyZonePrefab; // Área de daño en el suelo
    public float holyZoneDelay = 1.5f; // tiempo entre zonas
    public int holyZoneCount = 3;

    [Header("Castigo Divino")]
    public GameObject divineRayPrefab; // columna de energía
    public float divineDelay = 1f;

    [Header("Rendición (Absorción)")]
    public GameObject orbPrefab;
    public float pullForce = 5f;
    public int orbCountBase = 4;
    public float orbCooldown = 2f;

    // Movimiento auxiliar (opcional)
    private bool _isDashing = false;

    // ==================== MÉTODOS ====================

    void Start()
    {
        _currentHealth = _maxHealth;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);
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

    // ==================== VIDA ====================
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

    private void Die()
    {
        StopAllCoroutines();
        Destroy(gameObject);
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

    // ==================== FASES DE ATAQUE ====================

    IEnumerator PatronAtaque()
    {
        // Alternar ataques: Suelo Sagrado  Castigo Divino  Rendición
        yield return Phase1();
        yield return new WaitForSeconds(1f);

        yield return Phase2();
        yield return new WaitForSeconds(1f);

        yield return Phase3();
        yield return new WaitForSeconds(1f);
        /*
        while (_currentHealth > 0)
        {
            float lifePercent = (float)_currentHealth / _maxHealth;

            if (lifePercent > 0.66f)
                yield return Phase1();
            else if (lifePercent > 0.33f)
                yield return Phase2();
            else
                yield return Phase3();

            yield return new WaitForSeconds(2f);
        }
        */
    }

    // ------------------- FASE 1 -------------------
    IEnumerator Phase1()
    {
        Debug.Log("Fase 1: Suelo Sagrado");
        yield return HolyGroundAttack(1);
        yield return new WaitForSeconds(2f);
    }

    // ------------------- FASE 2 -------------------
    IEnumerator Phase2()
    {
        Debug.Log("Fase 2: Castigo Divino");
        yield return DivinePunishment(2);
        yield return new WaitForSeconds(2f);
    }

    // ------------------- FASE 3 -------------------
    IEnumerator Phase3()
    {
        Debug.Log("Fase 3: Rendición");
        yield return SurrenderAttack(3);
        yield return new WaitForSeconds(2f);
    }

    // ==================== ATAQUES ====================

    // --- Suelo Sagrado ---
    IEnumerator HolyGroundAttack(int phase)
    {
        int count = holyZoneCount + phase;
        for (int i = 0; i < count; i++)
        {
            Vector3 spawnPos = player.position + new Vector3(
                Random.Range(-3f, 3f),
                Random.Range(-3f, 3f),
                0f);

            Instantiate(holyZonePrefab, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(holyZoneDelay - (phase * 0.2f));
        }
    }

    // --- Castigo Divino ---
    IEnumerator DivinePunishment(int phase)
    {
        int count = Mathf.Clamp(phase, 1, 3);
        for (int i = 0; i < count; i++)
        {
            Vector3 target = player.position;
            GameObject indicator = Instantiate(divineRayPrefab, target, Quaternion.identity);

            yield return new WaitForSeconds(divineDelay - (phase * 0.1f));

            // Si el prefab tiene script DivineRay
          

            yield return new WaitForSeconds(0.5f);
        }
    }

    // --- Rendición (Atracción + Orbes) ---
    IEnumerator SurrenderAttack(int phase)
    {
        float duration = 1.5f + (phase * 0.5f);
        float timer = 0;

        while (timer < duration)
        {
            Vector3 dir = (transform.position - player.position).normalized;
            player.position += dir * pullForce * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }

        int orbs = orbCountBase + phase * 2;
        for (int i = 0; i < orbs; i++)
        {
            float angle = i * (360f / orbs);
            Quaternion rot = Quaternion.Euler(0, 0, angle);
            Instantiate(orbPrefab, transform.position, rot);
        }

        yield return new WaitForSeconds(orbCooldown);
    }

    // ==================== UTILIDAD ====================
    private void StopAttack()
    {
        StopAllCoroutines();
        _canFight = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerBulletDamage);
            Destroy(other.gameObject);
        }
    }
}
