using UnityEngine;

public class Ghost_Attack : MonoBehaviour
{
    [Header("Ataque a distancia")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private LayerMask _obstacleMask;

    [Header("Animación")]
    private Animator _animator;

    private Transform _player;
    private float _nextFireTime;
    private Vector2 _lastShotDirection;

    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_player != null)
            TryAttack();
    }

    private void TryAttack()
    {
        if (Time.time < _nextFireTime) return;

        float distanceToPlayer = Vector2.Distance(transform.position, _player.position);
        if (distanceToPlayer > _attackRange) return;

        Vector2 direction = (_player.position - transform.position).normalized;
        float distance = Vector2.Distance(_firePoint.position, _player.position);

        RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, direction, distance, _obstacleMask);
        if (hit.collider != null)
        {
            Debug.DrawLine(_firePoint.position, _player.position, Color.red);
            return;
        }

        // Guarda dirección y dispara animación
        _lastShotDirection = direction;
        int dirIndex = GetDirectionIndex(direction);
        _animator.SetInteger("Direction", dirIndex);
        _animator.SetTrigger("Attack");

        _nextFireTime = Time.time + _fireRate;

        Debug.DrawLine(_firePoint.position, _player.position, Color.green);
    }

    // Llamado desde Animation Event
    public void ShootProjectileFromAnimation()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);
        E_Projectile fireball = projectile.GetComponent<E_Projectile>();
        if (fireball != null)
            fireball.SetDirection(_lastShotDirection);
    }

    private int GetDirectionIndex(Vector2 dir)
    {
        // 1 = Down, 2 = Left, 3 = Right, 0 = Up
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? 3 : 2;
        else
            return dir.y > 0 ? 0 : 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }
}
