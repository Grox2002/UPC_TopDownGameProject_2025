using UnityEngine;

public class Arrow : MonoBehaviour
{
    //Variables
    [Header("General Configuration")]
    [SerializeField] private float _speed = 10f;
    [SerializeField] private int _damage = 10;

    private float lifeTime = 5f;

    private Rigidbody2D _rb;
    private Vector2 _direction;

    [Header("Sonido")]
    [SerializeField] private AudioClip _arrowSound; // sonido de la flecha
    [SerializeField] private float _volume = 0.8f;

    //Metodos

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (_arrowSound != null)
            AudioSource.PlayClipAtPoint(_arrowSound, transform.position, _volume);

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
            other.GetComponent<E_Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Elite"))
        {
            other.GetComponent<EliteHealth>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Boss"))
        {
            other.GetComponent<SeraphBoss>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

}
