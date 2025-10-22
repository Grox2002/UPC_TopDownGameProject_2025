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
    public bool IsMoving => _currentDirection != Vector2.zero;
    public bool canMove = true;

    private InputAction moveAction;
    private PlayerInput playerInput;
    public Vector2 LookDirection { get; private set; }

    [Header("Dash")]
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashDuration = 0.2f;
    [SerializeField] private float dashCooldown = 1f;
    private bool isDashing = false;
    private float lastDashTime;
    
    [SerializeField] private GameObject _bow;
    [SerializeField] private P_Attack _playerAttack;



    // Métodos
    private void Start()
    {
        _rb2D = GetComponent<Rigidbody2D>();

        _animator = GetComponent<Animator>();

        playerInput = GetComponent<PlayerInput>();

        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        //ActiveWalkAnimations();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }

        
        Vector2 moveInput = moveAction.ReadValue<Vector2>().normalized;

        _currentDirection = moveInput;


        if (canMove && !isDashing)
        {
            _rb2D.linearVelocity = moveInput * _walkingSpeed;
            ActiveWalkAnimations();
        }
        else if (!isDashing)
        {
            _rb2D.linearVelocity = Vector2.zero;
        }

        
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            Vector2 attackDir = (mousePos - (Vector2)transform.position).normalized;

            _animator.SetFloat("UltPosX", attackDir.x);
            _animator.SetFloat("UltPosY", attackDir.y);
            _lastDirection = attackDir;

            
            _animator.SetFloat("MovX", 0);
            _animator.SetFloat("MovY", 0);
        }


    }
    /*
    public void OnMove(InputValue value)
    {
        _currentDirection = value.Get<Vector2>().normalized;

        if (canMove && !isDashing)
        {
            _rb2D.linearVelocity = _currentDirection * _walkingSpeed;
        }
        else
          //_rb2D.linearVelocity = Vector2.zero;
    }
    */
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
            Vector2 MouseDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;
            _animator.SetFloat("UltPosX", MouseDirection.x);
            _animator.SetFloat("UltPosY", MouseDirection.y);
        }
    }
    
    private IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 dashDir = _currentDirection != Vector2.zero ? _currentDirection : _lastDirection;
        _rb2D.linearVelocity = dashDir * dashForce;

        yield return new WaitForSeconds(dashDuration);

        
        float t = 0f;
        while (t < 0.1f)
        {
            _rb2D.linearVelocity = Vector2.Lerp(_rb2D.linearVelocity, Vector2.zero, t / 0.1f);
            t += Time.deltaTime;
            yield return null;
        }

        _rb2D.linearVelocity = Vector2.zero;
        isDashing = false;
    }

}
