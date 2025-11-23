using UnityEngine;

public class Skeleton_Attack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private int _damage = 10;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackRange = 1.2f;
    [SerializeField] private Transform _attackOrigin; 

    private Animator _animator;
    private Transform _player;
    private float _lastAttackTime;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (_player == null) return;

        Vector2 origin = _attackOrigin ? _attackOrigin.position : transform.position;
        float distance = Vector2.Distance(origin, _player.position);

        // Ataca solo si el jugador está en rango y el cooldown terminó
        if (distance <= _attackRange && Time.time >= _lastAttackTime + _attackCooldown)
        {
            Attack(origin);
        }
    }

    private void Attack(Vector2 origin)
    {
        // Calcular dirección para animar hacia donde mira el enemigo
        Vector2 dir = (_player.position - (Vector3)origin).normalized;

        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            _animator.SetInteger("Direction", dir.x > 0 ? 3 : 2); // derecha o izquierda
        else
            _animator.SetInteger("Direction", dir.y > 0 ? 0 : 1); // arriba o abajo

        // Ejecuta la animación de ataque
        _animator.SetTrigger("Attack");

        _lastAttackTime = Time.time;
    }

    // Este método se llama desde un evento en la animación (En un frame específico)
    public void DealDamage()
    {
        if (_player == null) return;

        Vector2 origin = _attackOrigin ? _attackOrigin.position : transform.position;
        float distance = Vector2.Distance(origin, _player.position);

        // Aplica daño solo si el jugador sigue en rango
        if (distance <= _attackRange)
        {
            var playerHealth = _player.GetComponent<P_Health>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(_damage);
             
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = _attackOrigin ? _attackOrigin.position : transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, _attackRange);
    }

}
