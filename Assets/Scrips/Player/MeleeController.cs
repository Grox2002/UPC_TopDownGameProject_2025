using UnityEngine;
using System.Collections;


public class MeleeController : MonoBehaviour 
{
    [Header("Attack Origins")]
    [SerializeField] private Transform _attackOriginUp;
    [SerializeField] private Transform _attackOriginDown;
    [SerializeField] private Transform _attackOriginLeft;
    [SerializeField] private Transform _attackOriginRight;

    [Header("Configuración")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private float _hitboxDuration = 0.3f;
    [SerializeField] private float _meleeStaminaCost = 1f;
    [SerializeField] private LayerMask _enemyLayer;
    [SerializeField] private float _damage;

    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.15f;

    [Header("Parry")]
    [SerializeField] private GameObject _parryHitBoxUp;
    [SerializeField] private GameObject _parryHitBoxDown;
    [SerializeField] private GameObject _parryHitBoxLeft;
    [SerializeField] private GameObject _parryHitBoxRight;

    private float _nextAttackTime;
    private P_Attack _playerAttack;
    private Animator _animator;
    private Vector2 _lastAttackDirection;
    //private bool _isAttacking;

    private void Start()
    {
        _playerAttack = GetComponentInParent<P_Attack>();
        _animator = GetComponentInParent<Animator>();
    }

    // Este metodo es llamado desde P_Attack
    public void MeleeAttack()
    {
        if (Time.time < _nextAttackTime) return;
        if (!_playerAttack.ConsumeStamina(_meleeStaminaCost)) return;

        _nextAttackTime = Time.time + _attackCooldown;

        Vector2 dir;
        if (_playerAttack.IsPlayerMoving())
            dir = _playerAttack.GetLastDirection();
        else
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = (mouseWorldPos - (Vector2)transform.position).normalized;
        }

        _lastAttackDirection = dir;

        _animator.SetTrigger("MeleeAttack");
        SetAnimatorDirection(dir);
    }

    // Elige la animación segun la direccion
    private void SetAnimatorDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            _animator.SetInteger("Direction", dir.y > 0 ? 0 : 1); // 0=Up, 1=Down
        else
            _animator.SetInteger("Direction", dir.x > 0 ? 3 : 2); // 3=Right, 2=Left
    }

    // Llamado desde Animation Event
    public void DealMeleeDamage()
    {
        Transform origin = GetAttackOrigin(_lastAttackDirection);
        if (origin == null) return;

        Vector2 attackPos = origin.position;
        float radius = _attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPos, radius, _enemyLayer);
        foreach (var hit in hits)
        {
            E_Health health = hit.GetComponent<E_Health>();
            if (health != null)
                health.TakeDamage(_damage);

            // Empuje
            Rigidbody2D enemyRb = hit.GetComponent<Rigidbody2D>();
            if (enemyRb != null)
            {
                Vector2 knockbackDir = (hit.transform.position - transform.position).normalized;
                StartCoroutine(ApplyKnockback(enemyRb, knockbackDir));
            }

        }
    }

    private Transform GetAttackOrigin(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            return dir.y > 0 ? _attackOriginUp : _attackOriginDown;
        else
            return dir.x > 0 ? _attackOriginRight : _attackOriginLeft;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if (_attackOriginUp != null) Gizmos.DrawWireSphere(_attackOriginUp.position, _attackRange);
        if (_attackOriginDown != null) Gizmos.DrawWireSphere(_attackOriginDown.position, _attackRange);
        if (_attackOriginLeft != null) Gizmos.DrawWireSphere(_attackOriginLeft.position, _attackRange);
        if (_attackOriginRight != null) Gizmos.DrawWireSphere(_attackOriginRight.position, _attackRange);
    }

    // ---- PARRY ---- //
    public void ActivateParry()
    {
        GameObject hitbox = GetParryHitboxByDirection(_lastAttackDirection);
        if (hitbox != null)
            StartCoroutine(ActivateParryHitboxCoroutine(hitbox));
    }

    private GameObject GetParryHitboxByDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
            return dir.y > 0 ? _parryHitBoxUp : _parryHitBoxDown;
        else
            return dir.x > 0 ? _parryHitBoxRight : _parryHitBoxLeft;
    }

    private IEnumerator ActivateParryHitboxCoroutine(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(_hitboxDuration);
        hitbox.SetActive(false);
    }

    private IEnumerator ApplyKnockback(Rigidbody2D enemyRb, Vector2 dir)
    {
        // Guardar la velocidad actual
        Vector2 originalVelocity = enemyRb.linearVelocity;

        // Aplicar empuje 
        enemyRb.linearVelocity = dir * _knockbackForce;

        // Pausar movimiento del enemigo 
        E_Movement move = enemyRb.GetComponent<E_Movement>();
        if (move != null)
            move.enabled = false;

        yield return new WaitForSeconds(_knockbackDuration);

        enemyRb.linearVelocity = Vector2.zero;

        // Restaura control del enemigo
        if (move != null)
            move.enabled = true;
    }

}


