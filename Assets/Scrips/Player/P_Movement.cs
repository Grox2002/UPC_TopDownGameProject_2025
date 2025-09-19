using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class P_Movement : MonoBehaviour
{
    //Variables
    [Header("Movement Configuration")]
    [SerializeField] private float _walkingSpeed = 3f;
    [SerializeField] private Vector2 _currentDirection;
    public Vector2 _lastDirection;

    private Rigidbody2D _rb2D;
    private Animator _animator;
    private float _currentSpeed;

    [Header("Sprint")]
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _maxStamina = 3f;
    [SerializeField] private float _staminaRecoveryRate = 1f;

    private float _currentStamina;
    private bool _isExhausted = false;
    private bool _isSprinting = false;

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime;


    [SerializeField] private GameObject _bow;

    
    
    // Métodos
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        ActiveWalkAnimations();
        Run();
        UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
        _bow.SetActive(_currentDirection == Vector2.zero);

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

    }

    private void FixedUpdate()
    {
        if (!isDashing)
        {
            _rb2D.MovePosition(_rb2D.position + _currentDirection * _currentSpeed * Time.fixedDeltaTime);
        }
       

    }

    public void OnMove(InputValue value)
    {
        _currentDirection = value.Get<Vector2>().normalized;
       
    }

    public void OnSprint(InputValue value)
    {
        _isSprinting = value.isPressed;
    }

    private void ActiveWalkAnimations()
    {
        _animator.SetFloat("MovX", _currentDirection.x);
        _animator.SetFloat("MovY", _currentDirection.y);

        if (_currentDirection != Vector2.zero)
        {
            _lastDirection = _currentDirection;
            _animator.SetFloat("UltPosX", _currentDirection.x);
            _animator.SetFloat("UltPosY", _currentDirection.y);
        }
        else
        {
            // Si está quieto, mirar hacia el mouse
            Vector2 MouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            _animator.SetFloat("UltPosX", MouseDirection.x);
            _animator.SetFloat("UltPosY", MouseDirection.y);
        }

    }

    private void Run()
    {
        if (_isSprinting && !_isExhausted && _currentStamina > 0)
        {
            _currentSpeed = _runSpeed;
            _currentStamina -= Time.deltaTime;
            if (_currentStamina <= 0)
            {
                _currentStamina = 0;
                _isExhausted = true;
            }
        }
        else
        {
            _currentSpeed = _walkingSpeed;
            if (_currentStamina < _maxStamina)
            {
                _currentStamina += _staminaRecoveryRate * Time.deltaTime;
                if (_currentStamina >= _maxStamina)
                {
                    _currentStamina = _maxStamina;
                    _isExhausted = false;
                }
            }
        }
    }

    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        //Vector2 dashDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        //if (dashDir == Vector2.zero) dashDir = transform.right; // fallback

        Vector2 dashDir = _currentDirection;
        if (dashDir == Vector2.zero) dashDir = _lastDirection;

        _rb2D.linearVelocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashDuration);

        _rb2D.linearVelocity = Vector2.zero;
        isDashing = false;
    }

}
/*
    //Metodos
    void Start()
    {
        _rb2D = GetComponent <Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _currentStamina = _maxStamina;
    }
  
    private void Update()
    {
        ActiveWalkAnimations();

        Run();

        UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
    }

    private void FixedUpdate()
    {
        _rb2D.MovePosition(_rb2D.position + _currentDirection * _currentSpeed * Time.fixedDeltaTime);
    }


    //Animaciones de movimiento
    private void ActiveWalkAnimations()
    {
        _movX = Input.GetAxisRaw("Horizontal");
        _movY = Input.GetAxisRaw("Vertical");

        _currentDirection = new Vector2(_movX, _movY).normalized;

        _animator.SetFloat("MovX", _movX);
        _animator.SetFloat("MovY", _movY);

        if (_movX != 0 || _movY != 0)
        {
            _lastDirection = new Vector2(_movX, _movY);
            _animator.SetFloat("UltPosX", _movX);
            _animator.SetFloat("UltPosY", _movY);
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !_isExhausted && _currentStamina > 0)
        {
            _currentSpeed = _runSpeed;
            _currentStamina -= Time.deltaTime;
            if (_currentStamina <= 0)
            {
                _currentStamina = 0;
                _isExhausted = true;
            }
        }
        else
        {
            _currentSpeed = _walkingSpeed;
            if (_currentStamina < _maxStamina)
            {
                _currentStamina += _staminaRecoveryRate * Time.deltaTime;
                if (_currentStamina >= _maxStamina)
                {
                    _currentStamina = _maxStamina;
                    _isExhausted = false;
                }
            }
        }
    }

}
*/
