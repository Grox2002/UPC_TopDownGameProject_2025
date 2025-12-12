using UnityEngine;

public class Bow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    [Header("Config")]
    [SerializeField] private float _fireRate = 0.3f;
    [SerializeField] private float _nextFireTime = 0f;
    [SerializeField] private float _shootStaminaCost = 1f;
    public float ShootStaminaCost => _shootStaminaCost;

    private P_Attack _playerAttack;

    private void Awake()
    {
        _playerAttack = GetComponentInParent<P_Attack>();
    }


    public bool CanShoot()
    {
        return Time.time >= _nextFireTime;
    }


    public void Shoot()
    {
        GameObject arrow = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);
        arrow.GetComponent<Arrow>()?.SetDirection(_firePoint.right);

        _nextFireTime = Time.time + _fireRate;
    }

    
}
