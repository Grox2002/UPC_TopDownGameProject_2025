using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.AI;

public class E_Movement : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _stopDistance = 1f;
    [SerializeField] private float _detectionRange = 5f;

    [Header("Puntos de referencia")]
    [SerializeField] private Transform _stopOrigin;

    private NavMeshAgent _agent;
    private Transform _player;
    private Animator _animator;


    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;

        // Ajustes para 2D
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _speed;
        _agent.stoppingDistance = _stopDistance;
        _agent.autoBraking = true;
    }

    private void Update()
    {
        if (_player == null) return;

        HandleMovement();
        HandleAnimation();
    }

    private void HandleMovement()
    {
        if (_agent == null || !_agent.isOnNavMesh) return;

        Vector2 origin2D = _stopOrigin ? (Vector2)_stopOrigin.position : (Vector2)transform.position;
        Vector2 player2D = _player.position;

        float distance = Vector2.Distance(origin2D, player2D);

        if (distance > _detectionRange || distance <= _stopDistance)
        {
            StopMoving();
            return;
        }

        // Perseguir al jugador
        _agent.isStopped = false;
        Vector3 targetPos = new Vector3(_player.position.x, _player.position.y, transform.position.z);
        _agent.SetDestination(targetPos);
    }

    private void HandleAnimation()
    {
        if (_animator == null || _agent == null) return;

        Vector2 dir;
        float speed = _agent.velocity.magnitude;

        if (speed < 0.01f)
        {
            dir = (_player.position - transform.position).normalized;
        }
        else
        {
            dir = _agent.velocity.normalized;
        }

        _animator.SetFloat("MovX", dir.x);
        _animator.SetFloat("MovY", dir.y);
        _animator.SetFloat("UltPosX", dir.x);
        _animator.SetFloat("UltPosY", dir.y);
        _animator.SetFloat("Speed", speed);
    }

    private void StopMoving()
    {
        if (_agent == null) return;

        _agent.ResetPath();
        _agent.isStopped = true;
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 origin = _stopOrigin ? _stopOrigin.position : transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(origin, _detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(origin, _stopDistance);
    }
}
