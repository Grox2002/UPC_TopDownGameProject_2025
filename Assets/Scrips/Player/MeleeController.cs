using UnityEngine;
using System.Collections;


public class MeleeController : MonoBehaviour 
{
    //Variables
    private Animator _animator;

    [Header("Attack Points")]
    [SerializeField] private GameObject _hitBoxUp;
    [SerializeField] private GameObject _hitBoxDown;
    [SerializeField] private GameObject _hitBoxLeft;
    [SerializeField] private GameObject _hitBoxRight;
  
    [SerializeField] private float _attackCooldown = 0.5f;
    [SerializeField] private float _hitboxDuration = 0.2f;


    private float _nextAttackTime;

    [SerializeField] private Rigidbody2D _playerRb;
    [SerializeField] private P_Movement _movementRef;


    //[SerializeField] private GameObject _bow;

    //Metodos
    public void MeleeAttack()
    {
        _nextAttackTime = Time.time + _attackCooldown;

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = (mouseWorldPos - transform.position).normalized;

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
        DisableAllHitboxes();
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



    /*
     //Metodo antiguo
    private void Start()
    {
        DisableAllHitboxes();

        _animator = GetComponentInParent<Animator>();

        //_playerRb = GetComponentInParent<Rigidbody2D>();

    }

    private void Update()
    {
        MeleeAttack();

    }


    void DisableAllHitboxes()
    {
        _hitBoxUp.SetActive(false);
        _hitBoxLeft.SetActive(false);
        _hitBoxDown.SetActive(false);
        _hitBoxRight.SetActive(false);
    }

    
    public void MeleeAttack()
    {
        Vector2 dir = _playerRb.linearVelocity;

        if (Time.time < _nextAttackTime) return;

        if (dir.y > 0 && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxUp));
            //_animator.SetInteger("Direction", 0);
            _animator.SetTrigger("MeleeAttack");
            _animator.SetInteger("Direction", 0);
        }
        else if (dir.y < 0 && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxDown));
            _animator.SetTrigger("MeleeAttack");
            _animator.SetInteger("Direction", 1);
        }
        else if (dir.x < 0 && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxLeft));
            _animator.SetTrigger("MeleeAttack");
            _animator.SetInteger("Direction", 2);
        }
        else if (dir.x > 0 && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxRight));
            _animator.SetTrigger("MeleeAttack");
            _animator.SetInteger("Direction", 3);
        }

        _nextAttackTime = Time.time + _attackCooldown;
    }

    
  

    private IEnumerator ActivateHitboxTemporarily(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitbox.SetActive(false);
    }

    */

}


