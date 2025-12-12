using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class P_Movement : MonoBehaviour
{
    [Header("Movement Configuration")]
    [SerializeField] private float _walkingSpeed = 3f;
    private Vector2 _currentDirection;
    private Vector2 _targetVelocity;
    public Vector2 _lastDirection;

    public bool IsMoving => _currentDirection != Vector2.zero;
    public bool canMove = true;
    private Rigidbody2D _rb2D;
    private Animator _animator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    public bool overrideLook = false;


    [Header("Dash")]
    [SerializeField] private float dashForce = 12f;
    [SerializeField] private float dashDuration = 0.18f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime;


    [Header("Sonido")]
    [SerializeField] private AudioSource _dashSound;




    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];

    }
    private void Update()
    {
        HandleInput();
        HandleDashInput();
        HandleAnimations();
    }
    private void FixedUpdate()
    {
        if (!isDashing)
            _rb2D.linearVelocity = _targetVelocity;
    }

    private void HandleInput()
    {
        if (isDashing) return;

        _currentDirection = moveAction.ReadValue<Vector2>().normalized;

        if (canMove)
            _targetVelocity = _currentDirection * _walkingSpeed;
        else
            _targetVelocity = Vector2.zero;
    }


    private void HandleDashInput()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame &&
            canMove &&
            IsMoving &&
            Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash()
    {
        _dashSound.Play();

        isDashing = true;
        lastDashTime = Time.time;

        Vector2 dashDir = (_currentDirection != Vector2.zero ? _currentDirection : _lastDirection).normalized;
        _rb2D.linearVelocity = dashDir * dashForce;

        float time = 0f;
        while (time < dashDuration)
        {
            time += Time.deltaTime;
            yield return null;
        }

        _rb2D.linearVelocity = Vector2.zero;

        isDashing = false;
    }


    private void HandleAnimations()
    {
        if (!overrideLook)
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
                Vector2 MouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
                _animator.SetFloat("UltPosX", MouseDirection.x);
                _animator.SetFloat("UltPosY", MouseDirection.y);
            }
        }
    }

}
