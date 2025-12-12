using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;



public class P_Attack : MonoBehaviour
{
    //===========================//Variables//====================================//
    [Header("References")]
    [SerializeField] private Bow _bow;
    [SerializeField] private GameObject _bowObject;
    [SerializeField] private Animator _bowAnim;
    private Animator _anim;
    private MeleeController _melee;
    private P_Movement _movement;


    [Header("Stamina")]
    [SerializeField] private float normalRegenDelay = 0.7f;
    [SerializeField] private float _exaustedDuration = 2.1f;
    private float _maxStamina;
    public float _currentStamina;
    private float _lastStaminaUseTime;
    private float _currentRegenDelay;
    private bool _isExhausted;
    public bool canAttack = true;


    [Header("Audio")]
    [SerializeField] private AudioSource _exhaustedSound;


    private void Start()
    {
        #if !UNITY_EDITOR
            Cursor.visible = false; 
        #endif
    }

    private void Awake()
    {
        _movement = GetComponent<P_Movement>();
        _melee = GetComponentInChildren<MeleeController>();
        _anim = GetComponent<Animator>();

        _maxStamina = PlayerStats.Instance.maxStamina;
        _currentStamina = _maxStamina;

        _currentRegenDelay = normalRegenDelay;
    }

    private void Update()
    {
        // ---------- EXHAUSTION ----------
        if (!_isExhausted && _currentStamina <= 1)
        {
            StartCoroutine(ExhaustedRoutine());
        }

        // ---------- INPUT ----------
        if (!_isExhausted && canAttack)
        {
            if (Input.GetMouseButtonDown(0))
                TryRangedAttack();

            if (Input.GetMouseButtonDown(1))
                TryMeleeAttack();
        }

        // ---------- STAMINA REGEN ----------
        if (Time.time >= _lastStaminaUseTime + _currentRegenDelay && _currentStamina < _maxStamina && !_isExhausted)
        {
            _currentStamina += PlayerStats.Instance.staminaRecovery * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _maxStamina);
            UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);

        }
    }

    //=====================================
    //             RANGED
    //=====================================
    private void TryRangedAttack()
    {
        if (!_bow.CanShoot()) return;

        if (!ConsumeStamina(_bow.ShootStaminaCost))
            return;

        _bowObject.SetActive(true);
        _movement.overrideLook = true;

        LookAtTheMouse();

        _movement.canMove = false;
        _bow.Shoot();
        _bowAnim.SetTrigger("Shoot");

        StartCoroutine(RestoreMovementAfter(0.3f));
    }

    //=====================================
    //             MELEE
    //=====================================
    private void TryMeleeAttack()
    {
        if (!ConsumeStamina(_melee.GetMeleeCost())) return;

        _bowObject.SetActive(false);
        _movement.canMove = false;
        _melee.MeleeAttack();

        StartCoroutine(RestoreMovementAfter(0.3f));
    }
    public void ActivateDamageAndParry()
    {
        _melee.DealDamageAndParry();
    }

    //=====================================
    //           STAMINA
    //=====================================
    public bool ConsumeStamina(float cost)
    {
        if (_currentStamina < cost) return false;

        _currentStamina -= cost;
        _currentStamina = Mathf.Max(_currentStamina, 0f);

        _lastStaminaUseTime = Time.time;
        UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
        return true;
    }

    //=====================================
    //           EXHAUSTION
    //=====================================
    private IEnumerator ExhaustedRoutine()
    {
        _isExhausted = true;
        canAttack = false;
        _exhaustedSound.Play();

        _bow.enabled = false;
        _melee.enabled = false;

        yield return new WaitForSeconds(_exaustedDuration);

        _bow.enabled = true;
        _melee.enabled = true;

        _exhaustedSound.Stop();
        canAttack = true;
        _isExhausted = false;
        _currentRegenDelay = normalRegenDelay;
        _currentStamina = 1.1f;

    }


    //restaurar movimiento
    private IEnumerator RestoreMovementAfter(float time)
    {
        yield return new WaitForSeconds(time);
        _movement.canMove = true;
        _movement.overrideLook = false;
    }

    //Mirrar hacia el mouse
    private void LookAtTheMouse()
    {
        Vector2 mouseDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
        _anim.SetFloat("UltPosX", mouseDir.x);
        _anim.SetFloat("UltPosY", mouseDir.y);
        
        _anim.SetFloat("MovX", 0f);
        _anim.SetFloat("MovY", 0f);
    }

}






