using UnityEngine;
using System.Collections;


public class MeleeController : MonoBehaviour 
{
    //Variables
    [Header("Attack Points")]
    [SerializeField] private GameObject _hitBoxUp;
    [SerializeField] private GameObject _hitBoxDown;
    [SerializeField] private GameObject _hitBoxLeft;
    [SerializeField] private GameObject _hitBoxRight;
  
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private float _hitboxDuration = 0.3f;
    [SerializeField] private float _meleeStaminaCost = 1f;

    private float _nextAttackTime;
    private P_Attack _playerAttack;
    private Animator _animator;


    //Metodos
    private void Start()
    {
        DisableAllHitboxes();

        _playerAttack = GetComponentInParent<P_Attack>();

        _animator = GetComponentInParent<Animator>();
    }
    
    public void MeleeAttack()
    {
        if (Time.time < _nextAttackTime) return;
        if (!_playerAttack.ConsumeStamina(_meleeStaminaCost)) return;

        _nextAttackTime = Time.time + _attackCooldown;

        Vector2 dir;

        if (_playerAttack.IsPlayerMoving())
        {
            dir = _playerAttack.GetLastDirection();
        }
        else
        {
            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            dir = (mouseWorldPos - (Vector2)transform.position).normalized;
        }

        MeleeAttackAnimation(dir);

        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            if (dir.y > 0)
                StartCoroutine(ActivateHitbox(_hitBoxUp));
            else
                StartCoroutine(ActivateHitbox(_hitBoxDown));
        }
        else
        {
            if (dir.x > 0)
                StartCoroutine(ActivateHitbox(_hitBoxRight));
            else
                StartCoroutine(ActivateHitbox(_hitBoxLeft));
        } 
    }
    
    private IEnumerator ActivateHitbox(GameObject hitbox)
    {

        hitbox.SetActive(true);

        yield return new WaitForSeconds(_hitboxDuration);

        hitbox.SetActive(false);
        
    }

    private void DisableAllHitboxes()
    {
        _hitBoxUp.SetActive(false);
        _hitBoxDown.SetActive(false);
        _hitBoxLeft.SetActive(false);
        _hitBoxRight.SetActive(false);
    }

    private void MeleeAttackAnimation(Vector2 dir)
    {
        _animator.SetTrigger("MeleeAttack");

        if (Mathf.Abs(dir.y) > Mathf.Abs(dir.x))
        {
            _animator.SetInteger("Direction", dir.y > 0 ? 0 : 1);
        }
        else
        {
            _animator.SetInteger("Direction", dir.x > 0 ? 3 : 2);
        }
    }
}


