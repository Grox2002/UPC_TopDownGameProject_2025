using UnityEngine;

public class E_Projectile : MonoBehaviour
{
    [SerializeField] private float _damage;
    [SerializeField] private float _speed = 5f;

    private Vector2 _moveDirection;
    private Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void SetDirection(Vector2 direction)
    {
        _moveDirection = direction.normalized;

            
    }

    private void FixedUpdate()
    {
        if (_rb != null)
        {
            _rb.linearVelocity = _moveDirection * _speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<P_Health>().TakeDamage(_damage);
            Destroy(gameObject);
        }
        
        if (other.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Melee"))
        {
            if (gameObject.CompareTag("BossBullet"))
            {
                _moveDirection = -_moveDirection;
                _rb.linearVelocity = _moveDirection * _speed;

                gameObject.tag = "PlayerBullet";

                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = Color.yellow;
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
 
    }

}
