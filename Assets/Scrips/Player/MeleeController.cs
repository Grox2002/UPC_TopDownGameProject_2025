using System.Collections;
using UnityEngine;



public class MeleeController : MonoBehaviour 
{
    [Header("Attack Origins")]
    [SerializeField] private Transform _up;
    [SerializeField] private Transform _down;
    [SerializeField] private Transform _left;
    [SerializeField] private Transform _right;


    [Header("Attack Configuración")]
    [SerializeField] private float _attackRange = 1f;
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private float _meleeStaminaCost = 1f;
    [SerializeField] private LayerMask _damageableLayer;


    [Header("Parry")]
    [SerializeField] private float _parryDuration = 0.5f;


    [Header("Knockback")]
    [SerializeField] private float _knockbackForce = 5f;
    [SerializeField] private float _knockbackDuration = 0.15f;
    

    private float _nextAttack;
    private P_Attack _playerAttack;
    private Animator _animator;
    private Vector2 _lastDir;
    public bool _isAttacking;
    public float GetMeleeCost() => _meleeStaminaCost;

    [Header("Audio")]
    [SerializeField] private AudioSource _slashSound;


    private void Awake()
    {
        _playerAttack = GetComponentInParent<P_Attack>();
        _animator = GetComponentInParent<Animator>();

        DeactivateParryHitbox();
    }

    public void MeleeAttack()
    {
        if (Time.time < _nextAttack) return;
        if (!_playerAttack.ConsumeStamina(_meleeStaminaCost)) return;

        _nextAttack = Time.time + _attackCooldown;

        _slashSound.Play();

        _lastDir = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position).normalized;

        _animator.SetTrigger("MeleeAttack");
        SetDirectionParam(_lastDir);
    }

    private void SetDirectionParam(Vector2 dir)
    {
        int direction =
            Mathf.Abs(dir.y) > Mathf.Abs(dir.x) ?
                (dir.y > 0 ? 0 : 1) :  // up / down
                (dir.x > 0 ? 3 : 2);   // right / left

        _animator.SetInteger("Direction", direction);
    }

  
    public void DealDamageAndParry()
    {
        StartCoroutine(ActivateParryHitbox());
        DealMeleeDamage();
    }


    private Collider2D DirToParry(Vector2 dir)
    {
        Transform origin = DirToAttackOrigin(dir);
        return origin ? origin.GetComponent<Collider2D>() : null;
    }

    private Transform DirToAttackOrigin(Vector2 dir)
    {
        return Mathf.Abs(dir.y) > Mathf.Abs(dir.x)
            ? (dir.y > 0 ? _up : _down)
            : (dir.x > 0 ? _right : _left);
    }

    public void DealMeleeDamage()
    {
        Transform origin = DirToAttackOrigin(_lastDir);
        if (!origin) return;

        foreach (var hit in Physics2D.OverlapCircleAll(origin.position, _attackRange, _damageableLayer))
            DamageHit(hit);
    }

    private void DamageHit(Collider2D hit)
    {
        Vector2 dir = (hit.transform.position - transform.position).normalized;

        hit.GetComponent<E_Health>()?.TakeDamage(PlayerStats.Instance.meleeDamage);
        hit.GetComponent<Barrel>()?.TakeDamage(PlayerStats.Instance.meleeDamage);

        Rigidbody2D rb = hit.GetComponent<Rigidbody2D>();
        if (rb != null) StartCoroutine(DoKnockback(rb, dir));
    }


    private IEnumerator DoKnockback(Rigidbody2D rb, Vector2 dir)
    {
        rb.linearVelocity = dir * _knockbackForce;

        E_Movement move = rb.GetComponent<E_Movement>();
        if (move) move.enabled = false;

        yield return new WaitForSeconds(_knockbackDuration);

        rb.linearVelocity = Vector2.zero;
        if (move) move.enabled = true;
    }

    private IEnumerator ActivateParryHitbox()
    {
        Collider2D col = DirToParry(_lastDir);
        col.enabled = true;

        yield return new WaitForSeconds(_parryDuration);

        DeactivateParryHitbox();

    }
 
    public void DeactivateParryHitbox()
    {
        foreach (Transform t in new[] { _up, _down, _left, _right })
        {
            Collider2D col = t.GetComponent<Collider2D>();
            if (col != null)
                col.enabled = false;
        }
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Transform t in new[] { _up, _down, _left, _right })
            if (t != null)
                Gizmos.DrawWireSphere(t.position, _attackRange);
    }

}


