using UnityEngine;
using UnityEngine.UIElements;


public class ShootController : MonoBehaviour
{
    //Variables
    [SerializeField] private GameObject _projectilePrefab;
    [SerializeField] private Transform _bowTransform;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private float _fireRate = 0.5f;
    [SerializeField] private float _nextFireTime;

    [SerializeField]  private Animator _animator;

    [SerializeField] private float _shootStaminaCost = 1f;
    
    private P_Attack _playerAttack;

    //Metodos--------------------------------------------------------------------------------------------------------------------------
    private void Start()
    {
        gameObject.SetActive(false);

        _playerAttack = GetComponentInParent<P_Attack>();

        _animator = GetComponentInChildren<Animator>();

    }

    private void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        RotateBowTowardsMouse();
    }
    
    public void Shoot()
    {
        // Esto evita quel le disparo se ejecute si está en cooldown o sin stamina
        if (Time.time < _nextFireTime || !_playerAttack.ConsumeStamina(_shootStaminaCost))
            return;

        gameObject.SetActive(true);
        _animator.SetTrigger("Shoot");

        GameObject arrow = Instantiate(_projectilePrefab, _firePoint.position, _firePoint.rotation);

        arrow.GetComponent<Arrow>()?.SetDirection(_firePoint.right);

        _nextFireTime = Time.time + _fireRate;

    }
    
    //Esto rota el arco hacia donde esta el mouse :v
    public void RotateBowTowardsMouse()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mousePos - _bowTransform.position).normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _bowTransform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
     
    }
}


