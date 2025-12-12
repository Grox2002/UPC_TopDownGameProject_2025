using UnityEngine;

public class Ghost_Attack : MonoBehaviour
{
    [Header("Attack Configuration")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float attackRange = 5f;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private bool debugRay = true;

    private Animator animator;
    private Transform player;
    private float nextFireTime;
    private Vector2 lastShotDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (GameManager.Instance.IsPlayerDead || player == null) return;

        if (CanAttack())
            Attack();
    }

    private bool CanAttack()
    {
        // Fire rate
        if (Time.time < nextFireTime) return false;

        // Distance
        if (Vector2.Distance(transform.position, player.position) > attackRange)
            return false;

        // Obstacle check
        Vector2 dir = (player.position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, dir, Vector2.Distance(transform.position, player.position), obstacleMask);

        if (debugRay)
            Debug.DrawLine(firePoint.position, player.position, hit.collider != null ? Color.red : Color.green);

        return hit.collider == null;
    }

    private void Attack()
    {
        lastShotDirection = (player.position - transform.position).normalized;

        animator.SetInteger("Direction", GetDirection(lastShotDirection));
        animator.SetTrigger("Attack");

        nextFireTime = Time.time + fireRate;
    }

    public void ShootProjectileFromAnimation()
    {
        if (GameManager.Instance.IsPlayerDead) return;

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        if (projectile.TryGetComponent(out E_Projectile proj))
            proj.SetDirection(lastShotDirection);
    }

    private int GetDirection(Vector2 dir)
    {
        // 0=Up, 1=Down, 2=Left, 3=Right
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return dir.x > 0 ? 3 : 2;
        else
            return dir.y > 0 ? 0 : 1;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
