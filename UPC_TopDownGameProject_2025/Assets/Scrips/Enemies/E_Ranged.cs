using UnityEngine;


public class E_Ranged : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private float _attackRange;

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
            Shoot();
            _nextFireTime = Time.time + _fireRate;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _attackRange);
    }

}
