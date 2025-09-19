using UnityEngine;


public class ShootController : MonoBehaviour
{
    //Variables
    [SerializeField] private string _weaponName;
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFireTime;

    [SerializeField]  private Animator _animator;


    //Metodos
    public void Shoot()
    {
        if (Time.time >= _nextFireTime)
        {
            _animator.SetTrigger("Shoot");

            GameObject arrow = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

            _nextFireTime = Time.time + _fireRate;
        }
   
    }

    public void RotateBowTowardsMouse()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = (mouseWorldPos - _firePoint.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        _firePoint.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}


