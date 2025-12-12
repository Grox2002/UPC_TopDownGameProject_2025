using UnityEngine;

public class Arrow : MonoBehaviour
{
    //Variables
    [Header("General Configuration")]
    [SerializeField] private float _speed = 10f;
    //[SerializeField] private int _damage = 10;

    private float lifeTime = 2f;

    private Rigidbody2D _rb;
    private Vector2 _direction;

    [Header("Sonido")]
    [SerializeField] private AudioSource _arrowSound;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_arrowSound != null)
            _arrowSound.Play();

        Destroy(gameObject, lifeTime);
    }

    public void SetDirection(Vector2 dir)
    {
        _direction = dir.normalized;
    }

    void FixedUpdate()
    {
        _rb.linearVelocity = _direction * _speed;
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<E_Health>().TakeDamage(PlayerStats.Instance.shootDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Elite"))
        {
            other.GetComponent<EliteHealth>().TakeDamage(PlayerStats.Instance.shootDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Boss"))
        {
            other.GetComponent<SeraphBoss>().TakeDamage(PlayerStats.Instance.shootDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Drone"))
        {
            other.GetComponent<Eye_Health>().TakeDamage(PlayerStats.Instance.shootDamage);
            Destroy(gameObject);
        }
        else if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}
