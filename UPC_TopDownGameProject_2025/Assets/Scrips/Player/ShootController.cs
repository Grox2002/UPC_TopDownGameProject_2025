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

    private void Start()
    {
        //_animator = GetComponent<Animator>();
    }

    void Update()
    {
        Vector2 playerCurrentDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (playerCurrentDir != Vector2.zero)
        {
            _firePoint.right = playerCurrentDir; 
        }
    }

    public void Shoot()
    {
        if (Time.time >= _nextFireTime)
        {
            _animator.SetTrigger("Shoot");

            GameObject arrow = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

            _nextFireTime = Time.time + _fireRate;
        }
   

        // Esto es experimental - NO FUNCIONA BIEN
        /*
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0; 

        Vector3 direction = (mousePos - transform.position).normalized;
        
        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        */
    }
}


