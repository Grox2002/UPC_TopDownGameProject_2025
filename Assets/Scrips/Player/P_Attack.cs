using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class P_Attack : MonoBehaviour
{
    //===========================//Variables//====================================//
    [Header("Referencais")]
    [SerializeField] private ShootController _shootController;
    [SerializeField] private GameObject _bow;

    [SerializeField] private MeleeController _meleeController;

    private P_Movement _playerMovement;
    public Vector2 GetLastDirection() {return _playerMovement._lastDirection;}
    public bool IsPlayerMoving() => _playerMovement.IsMoving;

    [Header("Mira")]
    public Texture2D cursorTexture;
    [SerializeField] private Vector2 _cursorPos;

    private Animator _animator;


    [Header("Stamina System")]
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    [SerializeField] private float _lastStaminaUseTime;
    [SerializeField] private float _staminaRegenRate = 1f;
    [SerializeField] private float _staminaRegenDelay = 2f;

    //=========================//Metodos//========================================//
    private void Start()
    {
        _cursorPos = new Vector2(16, 16);
        
        Cursor.SetCursor(cursorTexture, _cursorPos, CursorMode.Auto);

        _playerMovement = GetComponent<P_Movement>();

        _currentStamina = _maxStamina;

        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            StartCoroutine(MeleeAttackRoutine());
        }
       
        if (Time.time >= _lastStaminaUseTime + _staminaRegenDelay && _currentStamina < _maxStamina)
        {
            _currentStamina += _staminaRegenRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _maxStamina);
            UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
        }
    }

    public bool ConsumeStamina(float cost)
    {
        if (_currentStamina >= cost)
        {
            _currentStamina -= cost;
            _lastStaminaUseTime = Time.time;
            UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
            return true;
        }
        return false;
    }

    //Ataque a distancia
    public void OnAttack(InputValue value)
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Vector2 attackDir = (mousePos - (Vector2)transform.position).normalized;

        // Actualizar animación hacia la dirección del ataque
        _animator.SetFloat("UltPosX", attackDir.x);
        _animator.SetFloat("UltPosY", attackDir.y);

        StartCoroutine(RangedAttackRoutine());
    }


    public IEnumerator MeleeAttackRoutine()
    {
        _playerMovement.canMove = false;

        _bow.SetActive(false);

        _meleeController.MeleeAttack();

        yield return new WaitForSeconds(0.3f);

        _playerMovement.canMove = true;
    }
    
    //Activa el hitbox de daño del ataque melee.
    public void ActivateHitboxFromAnimation()
    {
        if (_meleeController != null)
            _meleeController.DealMeleeDamage();
    }

    //Activa el hitbox del parry.
    public void ActivateHitBoxParryFromAnimation()
    {
        if (_meleeController != null)
            _meleeController.ActivateParry();
    }

    //Mientras se ejecute el ataque a distancia, se interumpe el movimiento.
    public IEnumerator RangedAttackRoutine()
    {
        _playerMovement.canMove = false;

        _shootController.Shoot();

        yield return new WaitForSeconds(0.3f);

        _playerMovement.canMove = true;

   
    }
}






