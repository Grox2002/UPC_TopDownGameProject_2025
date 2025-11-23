using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Splines;

public class SeraphBoss : MonoBehaviour
{
    //===============================VARIABLES====================================================//

    [Header("General Configuration")]
    [SerializeField] private int _maxHealth = 1000;
    private int _currentHealth;
    private int _currentPhase = 1;
    private bool _isInvulnerable = false;
    [SerializeField] private float attackInterval = 4f;

    [Header("References")]
    [SerializeField] private Transform player;
    private SpriteRenderer _spriteRenderer;
    private bool _canFight;
    private AudioSource _audioSource;
    [SerializeField] private GameObject _backgroundMusic;
    [SerializeField] private GameObject _bossHud;
    [SerializeField] private Animator _eyeAnimator;

    [SerializeField] private SeraphBoss _boss;
    [SerializeField] private Transform _bossFocus;
    [SerializeField] private DialogSO _bossDialog;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private CinemachineVirtualCameraBase _virtualCamera;
    [SerializeField] private Transform _playerFocus;


    [Header("Attack: Sacred Ground")]
    [SerializeField] private GameObject _sgZonesA;
    [SerializeField] private GameObject _sgZonesB;

    [Header("Attack: Divine Punishment")]
    public GameObject sacredLightningPrefab;
    private bool _divinePunishmentActivated = false;
    private GameObject _activeLaser;

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
    [SerializeField] private float orbCooldown = 2f;
    [SerializeField] private Transform firePoint;

    [Header("Eye Attack")]
    [SerializeField] private GameObject[] eyes;         
    [SerializeField] private GameObject _eyes;
    private bool _eyesActivated = false;

    [Header("Float Effect")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2f;
    private float _floatOffset;

    private bool _isTransitioning = false;

    [Header("Audio")]
    [SerializeField] private AudioSource _radialSource;
    [SerializeField] private AudioSource _roarSource;
    [SerializeField] private AudioSource _chargeSource;
    [SerializeField] private AudioSource _bossFightMusic;
    [SerializeField] private AudioSource _victoryAnthem;

    //===============================METODOS====================================================//

    private void Start()
    {
        _isInvulnerable = true;
        _currentHealth = _maxHealth;
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);
        _floatOffset = Random.Range(0f, Mathf.PI * 2f);
        StartCoroutine(FloatEffect());
        _initialPosition = transform.position;
        //_eyeAnimator.enabled = false;

    }

   

    //=================================================Attack System========================================================================================
    public void StartBattle()
    {
        _isInvulnerable = false;
        _bossHud.SetActive(true);
        _bossFightMusic.Play();
        _backgroundMusic.SetActive(false);

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
            yield return new WaitForSeconds(attackInterval);

            while (_isTransitioning)
                yield return null;

            // Determinar fase actual 
            int newPhase;
            if (_currentHealth > _maxHealth * 0.66f)
                newPhase = 1;
            else if (_currentHealth > _maxHealth * 0.33f)
                newPhase = 2;
            else
                newPhase = 3;

            // ===== Cambiar de fase  =====
            if (newPhase != _currentPhase)
            {
                StartCoroutine(PhaseTransition(newPhase));
                continue;
            }
            if (newPhase == 2)
            {
                attackInterval = 3f;
            }
            if (newPhase == 3 && !_eyesActivated)
            {
                StartCoroutine(DivinePunishmentCoroutine());
                attackInterval = 2f;
            }

            // ===== Definir ataques disponibles según fase =====
            List<System.Func<IEnumerator>> availableAttacks = new List<System.Func<IEnumerator>>();
            if (_currentPhase == 1)
                availableAttacks.Add(() => CelestialCharge());
            else if (_currentPhase == 2)
            {
                availableAttacks.Add(() => CelestialCharge());
                //availableAttacks.Add(() => DivinePunishmentCoroutine());
                //availableAttacks.Add(() => SpawnEye());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
            }
            else
            {
                //availableAttacks.Add(() => DivinePunishmentCoroutine());
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


    //==========================================================Health System=============================================================================
    public void TakeDamage(int damage)
    {
        if (_isInvulnerable) return;

        _currentHealth -= damage;
        UI_Manager.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        int newPhase;
        if (_currentHealth > _maxHealth * 0.66f)
            newPhase = 1;
        else if (_currentHealth > _maxHealth * 0.33f)
            newPhase = 2;
        else
            newPhase = 3;

        if (newPhase != _currentPhase)
        {
            
            StartCoroutine(PhaseTransition(newPhase));
            return;
        }

        
        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            Destroy(_activeLaser);
            _sgZonesA.SetActive(false);
            _sgZonesB.SetActive(false);
            _bossFightMusic.Stop();
            UI_Manager.Instance.HideBossHealthBar();
            StartCoroutine(BossDefeatSequence());
        }
        else
        {
            StartCoroutine(DamageEffect());
        }
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
   

    //===========================================================Phase Transition================================================================
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


    //=======================================================Sacred Ground Attack========================================================================
    private IEnumerator SpawnSacredZonesCoroutine()
    {
        _sgZonesA.SetActive(true);

        yield return new WaitForSeconds(4f);

        _sgZonesA.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _sgZonesB.SetActive(true);

        yield return new WaitForSeconds(4f);

        _sgZonesB.SetActive(false);
        
    }
    

    //======================================================Laser Atack========================================================================
    private IEnumerator DivinePunishmentCoroutine()
    {
        if (_divinePunishmentActivated) yield break;

        _divinePunishmentActivated = true; 

        //_LaserSource.Play();

        if (player == null) yield break;

        yield return new WaitForSeconds(0.5f);

        _activeLaser = Instantiate(sacredLightningPrefab, player.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);

       // Destroy(_activeLaser);
        yield break;
    }


    //=========================================================Charge Attack=========================================================================
    public IEnumerator CelestialCharge()
    {
        if (player == null || _isAttacking) yield break;
        _isAttacking = true;

        _chargeSource.Play();

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


    //======================================================Eye Attack===========================================================================================
    private IEnumerator SpawnEye()
    {
        _eyes.SetActive(true);

        yield return new WaitForSeconds(2f); //coldown ataque

        _eyeAnimator.SetTrigger("EyeAttack");

        yield return new WaitForSeconds(5f);

        

    }
   
    //======================================================Radial Attack====================================================================================
    public IEnumerator RadialAttack(int phase)
    {
        Debug.Log($"Iniciando ataque radial (fase {phase})");

        float totalDuration = 5f;        
        float shootInterval = 0.25f;       
        float shootTimer = 0f;
        float elapsed = 0f;
        float angleOffset = 0f;
        int bulletCount = 12;

        _roarSource.Play();
        _radialSource.Play();

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


    //======================================================Float Effect============================================================================
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


    //================================================Boss Defeated=======================================================================
    public IEnumerator BossDefeatSequence()
    {
        _virtualCamera.Follow = _bossFocus;
        _virtualCamera.LookAt = _bossFocus;

        yield return new WaitForSeconds(1f);

        _victoryAnthem.Play();

        _dialogManager.StartDialogue(_bossDialog);

        yield return new WaitUntil(() => _dialogManager.IsDialogueActive);

        yield return new WaitUntil(() => !_dialogManager.IsDialogueActive);

        yield return StartCoroutine(BossAscendAnimation());

        _virtualCamera.Follow = _playerFocus;
        _virtualCamera.LookAt = _playerFocus;

        yield return new WaitForSeconds(1f);

        GameManager.Instance.BossDefeated();

        Destroy(gameObject);
        
    }
    private IEnumerator BossAscendAnimation()
    {
        float duration = 2f;
        float timer = 0f;
        float speed = 3f;

        Transform bossTransform = _boss.transform;

        while (timer < duration)
        {
            bossTransform.position += Vector3.up * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}




