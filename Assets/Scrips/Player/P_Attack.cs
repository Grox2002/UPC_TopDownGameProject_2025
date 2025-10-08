using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System;

public class P_Attack : MonoBehaviour
{
    //Variables
    [SerializeField] private ShootController _shootController;
    [SerializeField] private GameObject _bow;

    [SerializeField] private MeleeController _meleeController;
    [SerializeField] private Parry _parry;

    private P_Movement _playerMovement;
    public Vector2 GetLastDirection() {return _playerMovement._lastDirection;}
    public bool IsPlayerMoving() => _playerMovement.IsMoving;

    public Texture2D cursorTexture;
    [SerializeField] private Vector2 _cursorPos;


    //Estamina
    [SerializeField] private float _maxStamina;
    [SerializeField] private float _currentStamina;
    [SerializeField] private float _lastStaminaUseTime;
    [SerializeField] private float _staminaRegenRate = 1f;
    [SerializeField] private float _staminaRegenDelay = 2f;

    //Metodos
    private void Start()
    {
        _cursorPos = new Vector2(16, 16);
        
        Cursor.SetCursor(cursorTexture, _cursorPos, CursorMode.Auto);

        _playerMovement = GetComponent<P_Movement>();

        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        //Ataque melee
        if (Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(MeleeAttackRoutine());
        }

        //Parry
        if (Input.GetMouseButtonDown(1))
        {
            _parry.ActiveParry();
        }

        //Regeneracion de estamian
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
        StartCoroutine(RangedAttackRoutine());
    }


    public IEnumerator MeleeAttackRoutine()
    {
        _playerMovement.CanMove = false;
        //_playerMovement.enabled = false;
        _bow.SetActive(false);
        _meleeController.MeleeAttack();

        yield return new WaitForSeconds(0.2f);

        //_playerMovement.enabled = true;
        _playerMovement.CanMove = true;
    }

    public IEnumerator RangedAttackRoutine()
    {
        //_bow.SetActive(true);
        _playerMovement.enabled = false;
        _shootController.Shoot();

        yield return new WaitForSeconds(0.2f);

        _playerMovement.enabled = true;
        //_bow.SetActive(false);
    }

}






