using UnityEngine;

public class Skeleton_Attack : MonoBehaviour
{
    [Header("Ataque")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private float attackRange = 1.2f;
    [SerializeField] private Transform attackOrigin;

    private Animator animator;
    private Transform playerTransform;
    private P_Health playerHealth;
    private float lastAttackTime;

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerHealth = player.GetComponent<P_Health>();
        }
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlayerDead || playerTransform == null) return;

        if (CanAttack())
            Attack();
    }

    private bool CanAttack()
    {
        Vector2 origin = attackOrigin ? (Vector2)attackOrigin.position : (Vector2)transform.position;
        float distance = Vector2.Distance(origin, playerTransform.position);

        return distance <= attackRange && Time.time >= lastAttackTime + attackCooldown;
    }

    private void Attack()
    {
        Vector2 origin = attackOrigin ? (Vector2)attackOrigin.position : (Vector2)transform.position;

        // Determinar dirección para laamiacion
        Vector2 dir = (playerTransform.position - (Vector3)origin).normalized;
        animator.SetInteger("Direction", GetDirection(dir));
        animator.SetTrigger("Attack");

        lastAttackTime = Time.time;
    }

    public void DealDamage()
    {
        if (GameManager.Instance.IsPlayerDead || playerTransform == null) return;

        Vector2 origin = attackOrigin ? (Vector2)attackOrigin.position : (Vector2)transform.position;
        if (Vector2.Distance(origin, playerTransform.position) <= attackRange)
            playerHealth.TakeDamage(damage);
    }

    private int GetDirection(Vector2 dir)
    {
        // 0=Arriba, 1=Abajo, 2=Izquierda, 3=Derecha
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? 3 : 2;
        else
            return dir.y > 0 ? 0 : 1;
    }

    private void OnDrawGizmosSelected()
    {
        Vector2 origin = attackOrigin ? (Vector2)attackOrigin.position : (Vector2)transform.position;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, attackRange);
    }

}
