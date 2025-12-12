using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

public class SeraphBoss : MonoBehaviour
{


    //===============================ATENCION=============================================================//

    //Vengo del pasado para recordarte que limpies este elefantiasico y aberrante codigo y lo separes en dos scripts
    //con responzabilidad delegada para ataques y centro de ejcucion de patrones a parte, quiza una clase
    //singleton que administre los ataques y un script central que llame los metodos cuando corresponda :v
    //Y dicho sea al paso, ¡ENCUENTRA UNA MANERA DE NO ABUSAR DE LOS SINGLETON CARAJOOOO!

    //====================================================================================================//


    //Pos data no toques nada del la lista de enumerador, si no el boss se pone como lokita y combina la logica
    //de daño y vida en una clase padre


    //===============================VARIABLES====================================================//

    [Header("General Configuration")]
    [SerializeField] private float _maxHealth = 1000;
    private float _currentHealth;
    private int _currentPhase = 1;
    private bool _phase2Triggered = false;
    private bool _phase3Triggered = false;
    private bool _isInvulnerable = false;
    private bool _isTransitioning = false;
    [SerializeField] private float attackInterval = 4f;
    [SerializeField] private int _playerProjectileDamage = 20;
    private Coroutine attackLoopRoutine;
    private Coroutine attackRoutine;
    private bool _attackInProgress = false;


    [Header("References")]
    [SerializeField] private GameObject _backgroundMusic; //Esto cambiarlo despues (Debo hacer un gestor de sonidos independiente, pero no tengo monster y estoy quemado)
    private Transform player;
    private SpriteRenderer _spriteRenderer;
    private bool _canFight;
    private Animator _animator;

    [SerializeField] private SeraphBoss _boss;
    [SerializeField] private Transform _bossFocus;
    [SerializeField] private DialogSO _bossDialog;
    [SerializeField] private BossEvent _bossEvent;

    [SerializeField] private CinemachineVirtualCameraBase _virtualCamera;
    [SerializeField] private Transform _startPosition;
    [SerializeField] private AnimationClip _attackAnimation;
    [SerializeField] private CinemachineImpulseSource _impulseSource;
    [SerializeField] private SpriteRenderer _wingsSR;
    

    [Header("Attack: Sacred Ground")]
    [SerializeField] private GameObject _sgZonesA;
    [SerializeField] private GameObject _sgZonesB;
    [SerializeField] private float _zoneDuration = 3f;


    [Header("Attack: Divine Punishment")]
    [SerializeField] private GameObject sacredLightningPrefab;
    [SerializeField] private float _laserDuration;
    [SerializeField] private GameObject _laserTarget;
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
    [SerializeField] private LineRenderer lineRenderer;
    
    public GameObject dashDamager;
    public LayerMask playerLayer;
    private bool _isAttacking = false;
    private Vector2 _initialPosition;

    [Header("Ataque: Radial Fire")]
    [SerializeField] private GameObject orbPrefab;
    [SerializeField] private float _orbCooldown = 2f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float _duration = 5f;
    [SerializeField] private float _interval = 0.25f;
    [SerializeField] private float _angleOffset = 0f;
    [SerializeField] private int _bulletCount = 12;

    [Header("Transition Fase")]
    [SerializeField] private GameObject[] _eyes;         
    [SerializeField] private GameObject _eyeContainer;
    [SerializeField] private float _healPerSecond;
    private bool _eyesActivated = false;
    private List<DroneEye> eyesAlive = new List<DroneEye>();

    [Header("Float Effect")]
    [SerializeField] private Transform visualTransform;
    [SerializeField] private float floatAmplitude = 0.25f;
    [SerializeField] private float floatSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioSource _radialSource;
    [SerializeField] private AudioSource _roarSource;
    [SerializeField] private AudioSource _chargeSource;
    [SerializeField] private AudioSource _bossFightMusic;
    [SerializeField] private AudioSource _fireImpact;
    [SerializeField] private AudioSource _laserCharged;
    [SerializeField] private AudioSource _seraphDamaged;
    [SerializeField] private AudioSource _healingSound;


    //===============================METODOS====================================================//


    private void Awake()
    {
        player = GameManager.Instance.playerTransform;

        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        _animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _isInvulnerable = true;
        _currentHealth = _maxHealth;

        BossUI.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        StartCoroutine(FloatEffect());

        _initialPosition = _startPosition.position;

        lineRenderer.positionCount = 2;
        lineRenderer.widthCurve = AnimationCurve.Constant(0, 1, 3f); 
        lineRenderer.widthMultiplier = 0.5f; 
        lineRenderer.enabled = false;
    }



    //=================================================Attack System========================================================================================
    public void StartBattle()
    {
        if (DialogManager.Instance.IsDialogueActive) return;

        BossUI.Instance.ShowBossHealthBar();

        _isInvulnerable = false;
        _bossFightMusic.Play();
        _backgroundMusic.SetActive(false);

        if (player != null && !_canFight)
        {
            _canFight = true;
            if (attackLoopRoutine == null)
                attackLoopRoutine = StartCoroutine(PatronAtaque());
        }
    }

    private IEnumerator PatronAtaque()
    {
        Debug.Log("PatronAtaque started for phase " + _currentPhase);
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);

            Debug.Log($"PatronAtaque tick - phase {_currentPhase} isTransitioning={_isTransitioning} attackInProgress={_attackInProgress}");

            // no atacar si está en transición
            if (_isTransitioning)
                continue;

            // Si ya hay un ataque en progreso, saltar
            if (_attackInProgress)
                continue;

            _attackInProgress = true;

            // Elegir lista según fase
            List<Func<IEnumerator>> availableAttacks = new List<Func<IEnumerator>>();

            if (_currentPhase == 1)
            {
                availableAttacks.Add(() => CelestialCharge());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
            }
            else if (_currentPhase == 2)
            {
                attackInterval = 3f;
                availableAttacks.Add(() => CelestialCharge());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
                availableAttacks.Add(() => RadialAttack());
            }
            else // phase 3
            {
                StartCoroutine(DivinePunishmentCoroutine());
                attackInterval = 2f;
                availableAttacks.Add(() => DivinePunishmentCoroutine());
                availableAttacks.Add(() => CelestialCharge());
                availableAttacks.Add(() => SpawnSacredZonesCoroutine());
                availableAttacks.Add(() => RadialAttack());
            }

            // Comenzar ataque seleccionado y esperar a que termine
            int index = UnityEngine.Random.Range(0, availableAttacks.Count);
            Debug.Log("Starting attack index " + index);
            attackRoutine = StartCoroutine(availableAttacks[index]());
            yield return attackRoutine;
            attackRoutine = null;

            _attackInProgress = false;
        }
    }


    //==========================================================Health System=============================================================================
    public void TakeDamage(float damage)
    {
        if (_isInvulnerable) return;

        _currentHealth -= damage;
        BossUI.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

        // Determinar fase según vida
        int newPhase;
        if (_currentHealth > _maxHealth * 0.66f)
            newPhase = 1;
        else if (_currentHealth > _maxHealth * 0.33f)
            newPhase = 2;
        else
            newPhase = 3;

        //====== ACTIVAR FASE 1 =======================
        if (newPhase == 2 && !_phase2Triggered)
        {
            _phase2Triggered = true;
            StartCoroutine(TransitionPhase(2));
            _currentPhase = 2;
            return;
        }

        //====== ACTIVAR FASE 3 =======================
        if (newPhase == 3 && !_phase3Triggered)
        {
            _phase3Triggered = true;
            StartCoroutine(TransitionPhase(3));
            _currentPhase = 3;
            return;
        }

        //========Detath=============================
        if (_currentHealth <= 0)
        {
            StopAllCoroutines();
            Destroy(_activeLaser);
            _sgZonesA.SetActive(false);
            _sgZonesB.SetActive(false);
            _bossFightMusic.Stop();
            BossUI.Instance.HideBossHealthBar();
            _bossEvent.BossIsDefeated();
        }
        else
        {
            //_animator.SetTrigger("StartDamageEffect");  // Esto quedo feo, arreglar despues si hay ganas :v
            StartCoroutine(DamageEffect());
        }
    }

    private IEnumerator DamageEffect()
    {
        _seraphDamaged.Play();

        Color originalColor = _spriteRenderer.color;
        Color wingsOriginalColor = _wingsSR.color;

        Color damageColor = new Color(1f, 1f, 1f, 0.5f); 

        for (int i = 0; i < 2; i++)
        {
            _spriteRenderer.color = damageColor;
            _wingsSR.color = damageColor;
            yield return new WaitForSeconds(0.1f);
            _spriteRenderer.color = originalColor;
            _wingsSR.color = originalColor;
            yield return new WaitForSeconds(0.05f);
        }
    }
   


    //=======================================================Sacred Ground Attack========================================================================
    private IEnumerator SpawnSacredZonesCoroutine()
    {
        if (_isTransitioning) yield break;

        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_attackAnimation.length);
        _fireImpact.Play();
        _sgZonesA.SetActive(true);
        
        yield return new WaitForSeconds(_zoneDuration);

        _sgZonesA.SetActive(false);

        yield return new WaitForSeconds(0.1f);

        _animator.SetTrigger("Attack");
        yield return new WaitForSeconds(_attackAnimation.length);
        _fireImpact.Play();
        _sgZonesB.SetActive(true);
       
        yield return new WaitForSeconds(_zoneDuration);

        _sgZonesB.SetActive(false);
        
    }
    

    //======================================================Laser Atack========================================================================
    private IEnumerator DivinePunishmentCoroutine()
    {
        if(_isTransitioning || _divinePunishmentActivated) yield break;

        _divinePunishmentActivated = true;

        GameObject target = Instantiate(_laserTarget, player.position, Quaternion.identity);
        _laserCharged.Play();

        yield return new WaitWhile(() => _laserCharged.isPlaying);

        Transform targetPos = target.transform;

        _activeLaser = Instantiate(sacredLightningPrefab, targetPos.position, Quaternion.identity);

        yield return new WaitForSeconds(0.5f);
        Destroy(target);

        yield return new WaitForSeconds(_laserDuration);

        Destroy(_activeLaser);
        _activeLaser = null;

        _divinePunishmentActivated = false;
        Debug.Log("DivinePunishment end");
    }


    //=========================================================Charge Attack=========================================================================
    public IEnumerator CelestialCharge()
    {
        if (_isTransitioning) yield break;
        if (player == null || _isAttacking) yield break;

        _isAttacking = true;

        _animator.SetTrigger("DashAttack");
        _chargeSource.Play();

        Vector2 startPos = transform.position;
        Vector2 riseTarget = startPos + Vector2.up * riseHeight;

        yield return MoveTo(riseTarget, riseSpeed);

        // Activa linea roja
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, player.position);

        float time = 0f;
        while (time < pauseBeforeDash)
        {
            lineRenderer.SetPosition(0, transform.position);
            lineRenderer.SetPosition(1, player.position);

            time += Time.deltaTime;
            yield return null;
        }
        lineRenderer.enabled = false;

        //Dash
        dashDamager.SetActive(true);
        _impulseSource.GenerateImpulse();
        Vector2 dashDir = (player.position - transform.position).normalized;
        Vector2 dashTarget = (Vector2)transform.position + dashDir * dashDistance;

        yield return MoveTo(dashTarget, dashSpeed);
        dashDamager.SetActive(false);

        Vector2 retreatTarget = (Vector2)transform.position - dashDir * retreatDistance;
        yield return MoveTo(retreatTarget, retreatSpeed);

        yield return MoveTo(_initialPosition, returnSpeed);

        _isAttacking = false;
    }
    private IEnumerator MoveTo(Vector2 target, float speed)
    {
        while (Vector2.Distance(transform.position, target) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            yield return null;
        }
    }

    //================================================= Transition Phase ================================================================
    private IEnumerator TransitionPhase(int newPhase)
    {
        Debug.Log("Enter TransitionPhase -> " + newPhase);

        _isTransitioning = true;
        _isInvulnerable = true;

        // Detener ataque activo
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }

        // Detener loop principal si esta corriendo
        if (attackLoopRoutine != null)
        {
            StopCoroutine(attackLoopRoutine);
            attackLoopRoutine = null;
        }

        // Limpiar estados
        _attackInProgress = false;
        _divinePunishmentActivated = false;
        _isAttacking = false;

        _sgZonesA.SetActive(false);
        _sgZonesB.SetActive(false);

        _eyesActivated = true;
        _roarSource.Play();
        StartCoroutine(RegenWhileEyesActive());

        _eyeContainer.SetActive(true);

        foreach (GameObject eye in _eyes)
            eye.SetActive(true);

        yield return new WaitForSeconds(2f);

        _currentPhase = newPhase;

        Debug.Log("TransitionPhase setup complete for phase " + newPhase + " - waiting for eyes to be cleared.");
    }
    public void CheckIfAllEyesInactive()
    {
        foreach (var eye in _eyes)
        {
            if (eye.activeSelf) return;
        }

        Debug.Log("All eyes inactive: finishing transition.");

        _isInvulnerable = false;
        _eyesActivated = false;
        _isTransitioning = false;
        _eyeContainer.SetActive(false);
        BossUI.Instance.HideTransitionBar();

        _attackInProgress = false;
        _divinePunishmentActivated = false;
        _isAttacking = false;

        // Reiniciar loop
        if (attackLoopRoutine == null)
        {
            attackLoopRoutine = StartCoroutine(PatronAtaque());
            Debug.Log("Attack loop restarted after transition.");
        }
    }
    private IEnumerator RegenWhileEyesActive()
    {
        BossUI.Instance.ShowTransitionBar();

        _healingSound.Play();

        while (_eyesActivated)
        {
            _currentHealth = Mathf.Min(_currentHealth + _healPerSecond, _maxHealth);
            BossUI.Instance.UpdateTransitionBar(_currentHealth, _maxHealth);
            BossUI.Instance.UpdateBossHealthBar(_currentHealth, _maxHealth);

            yield return new WaitForSeconds(1f);
        }

        BossUI.Instance.HideTransitionBar();
        _healingSound.Stop();
    }

    //======================================================Radial Attack====================================================================================
    public IEnumerator RadialAttack()
    {
        if (_isTransitioning) yield break;

        _animator.SetTrigger("Attack");
        _radialSource.Play();

        float currentOffset = _angleOffset; 
        float endTime = Time.time + _duration;

        while (Time.time < endTime)
        {
            if (_isTransitioning) yield break;

            ShootRadial(_bulletCount, currentOffset);
            currentOffset = (currentOffset + 10f) % 360f; 

            yield return new WaitForSeconds(_interval);
        }

        yield return new WaitForSeconds(_orbCooldown);
        Debug.Log("Ataque radial finalizado");
    }

    void ShootRadial(int count, float offset = 0f)
    {
        float step = 360f / count;

        for (int orb = 0; orb < count; orb++)
        {
            float angle = step * orb + offset;
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad),Mathf.Sin(angle * Mathf.Deg2Rad));

            Instantiate(orbPrefab, firePoint.position, Quaternion.identity).GetComponent<E_Projectile>().SetDirection(dir);
        }
    }


    //======================================================Float Effect============================================================================
    private IEnumerator FloatEffect()
    {
        Vector3 basePos = visualTransform.localPosition;

        while (true)
        {
            if (!_isAttacking)
            {
                float newY = Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
                visualTransform.localPosition = new Vector3(basePos.x, basePos.y + newY, basePos.z);
            }
            else
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerBullet"))
        {
            TakeDamage(_playerProjectileDamage);
            Destroy(other.gameObject);
        }
    }
}




