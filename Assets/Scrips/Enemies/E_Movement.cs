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
    private Rigidbody2D _rb;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _animator = GetComponent<Animator>();

        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.speed = _speed;
        _agent.stoppingDistance = _stopDistance;
        _agent.autoBraking = false;
    }

    void Update()
    {
        if (_player == null) return;

        Vector3 stopOrigin = _stopOrigin != null ? _stopOrigin.position : transform.position;
        float distance = Vector2.Distance(stopOrigin, _player.position);
        Vector2 direction = (_player.position - stopOrigin).normalized;

        if (distance <= _detectionRange)
        {
            if (distance > _stopDistance && _player != null && _agent != null)
            {
                _agent.isStopped = false;
                _agent.SetDestination(_player.position);
            }
            else
            {
                _agent.isStopped = true;
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
            }

            _animator.SetFloat("UltPosX", direction.x);
            _animator.SetFloat("UltPosY", direction.y);
        }
        else
        {
            _agent.isStopped = true;
            _agent.ResetPath();
            _agent.velocity = Vector3.zero;
        }

        float speed = _agent.velocity.magnitude;
        _animator.SetFloat("Speed", speed);

        if (speed > 0.01f)
        {
            _animator.SetFloat("MovX", direction.x);
            _animator.SetFloat("MovY", direction.y);
        }
        else
        {
            _animator.SetFloat("MovX", 0);
            _animator.SetFloat("MovY", 0);
        }

    }

    private void OnDrawGizmosSelected()
    {
        Vector3 stopOrigin = _stopOrigin != null ? _stopOrigin.position : transform.position;

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(stopOrigin, _stopDistance);
    }
}
