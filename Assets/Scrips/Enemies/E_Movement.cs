using UnityEngine;

public class E_Movement : MonoBehaviour
{
    //Variables
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _stopDistance = 1f;
    [SerializeField] private float _detectionRange;

    private SpriteRenderer _sr;
    private Transform _player;

    //Metodos
    void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void Update()
    {
        if (_player != null)
        {
            EnemyMovement();
        }

    }

    void EnemyMovement()
    {
        float playerDistance = Vector2.Distance(_player.position, transform.position);

        if (playerDistance <= _detectionRange)
        {
            Vector2 direction = _player.position - transform.position;

            if (playerDistance > _stopDistance)
            {
                direction.Normalize();
                transform.position += (Vector3)(direction * _speed * Time.deltaTime);
            }

            _sr.flipX = _player.position.x > transform.position.x;
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _detectionRange);
    }
}
