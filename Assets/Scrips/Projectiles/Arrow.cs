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
    [SerializeField] private AudioClip arrowSound; // sonido de la flecha
    [SerializeField] private float volume = 0.8f;

    //Metodos

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();

        if (arrowSound != null)
            AudioSource.PlayClipAtPoint(arrowSound, transform.position, volume);

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
