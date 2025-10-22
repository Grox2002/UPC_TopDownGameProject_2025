using UnityEngine;
using UnityEngine.AI;

public class E_Movement : MonoBehaviour
{
    [Header("Configuración de movimiento")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _stopDistance = 1f;    
    [SerializeField] private float _detectionRange = 5f; 

    private NavMeshAgent _agent;
    private Transform _player;
    private Animator _animator;

    private Vector2 _currentDirection; 
    private Vector2 _lastDirection;    

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _animator = GetComponent<Animator>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _speed;
    }

    void Update()
    {
        if (_player != null && _agent.isOnNavMesh)
        {
            HandleMovement();
            UpdateAnimations();
        }
    }

    private void HandleMovement()
    {
        float distance = Vector2.Distance(_player.position, transform.position);

        if (distance <= _detectionRange)
        {
            if (distance > _stopDistance)
            {
                // Caminar hacia el jugador
                _agent.SetDestination(_player.position);
            }
            else
            {
                // Dentro del rango de ataque: detenerse
                _agent.ResetPath();
            }
        }
        else
        {
            _agent.ResetPath(); // Fuera del rango de detección
        }
    }

    private void UpdateAnimations()
    {
        Vector2 toPlayer = (_player.position - transform.position).normalized;
        float distance = Vector2.Distance(_player.position, transform.position);

        bool isMoving = distance > _stopDistance && distance <= _detectionRange;

        // Mantiene la dirección hacia el jugador (siempre)
        _currentDirection = toPlayer;
        if (_currentDirection.magnitude > 0.01f)
            _lastDirection = _currentDirection;

        // Actualizar parámetros del Animator
        _animator.SetFloat("MovX", _currentDirection.x);
        _animator.SetFloat("MovY", _currentDirection.y);
        _animator.SetFloat("UltPosX", _lastDirection.x);
        _animator.SetFloat("UltPosY", _lastDirection.y);

        // Nuevo: controla si debe reproducir idle o walk
        _animator.SetBool("IsMoving", isMoving);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _stopDistance);
    }

}
