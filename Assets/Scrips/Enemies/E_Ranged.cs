using UnityEngine;


public class E_Ranged : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _obstacleMask;

    private float _nextFireTime;
    private Transform _player;

    //Metodos
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        
        if (_player != null)
        {
            EnemyAttack();
        }
    }

    void Shoot()
    {
        GameObject projectile = Instantiate(_projectilePrefab, _firePoint.position, Quaternion.identity);

        Vector2 shotDirection = (_player.position - _firePoint.position).normalized;

        FireBall fireball = projectile.GetComponent<FireBall>();
        if (fireball != null)
        {
            fireball.SetDirection(shotDirection);
        }

    }

    private void EnemyAttack()
    {
        float playerDistance = Vector2.Distance(transform.position, _player.position);

        if (Time.time >= _nextFireTime && playerDistance <= _attackRange)
        {
            Vector2 direction = (_player.position - _firePoint.position).normalized;
            float distance = Vector2.Distance(_firePoint.position, _player.position);

            RaycastHit2D hit = Physics2D.Raycast(_firePoint.position, direction, distance, _obstacleMask);

            // Dispara solo si no hay obstáculos en el camino
            if (hit.collider == null)
            {
                Shoot();
                _nextFireTime = Time.time + _fireRate;
            }

            // Debug visual
            Debug.DrawLine(_firePoint.position, _player.position, hit.collider == null ? Color.green : Color.red);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
        
    }



}
