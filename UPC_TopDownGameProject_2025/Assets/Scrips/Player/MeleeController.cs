using UnityEngine;
using System.Collections;


public class MeleeController : MonoBehaviour 
{
    private Animator _animator;

    [Header("Attack Points")]
    [SerializeField] private GameObject _hitBoxUp;
    [SerializeField] private GameObject _hitBoxDown;
    [SerializeField] private GameObject _hitBoxLeft;
    [SerializeField] private GameObject _hitBoxRight;
  
    [SerializeField] private float _attackCooldown = 0.5f;
   
    private float _nextAttackTime;
    

    private void Start()
    {
        DisableAllHitboxes();

        _animator = GetComponentInParent<Animator>();

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
        if (Time.time < _nextAttackTime) return;

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxUp));
            //_animator.SetInteger("Direction", 0);
            //_animator.SetTrigger("MeleeAttack");
        }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxDown));
            //_animator.SetInteger("Direction", 1);
            //_animator.SetTrigger("MeleeAttack");
        }
        else if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxLeft));
            //_animator.SetInteger("Direction", 2);
            //_animator.SetTrigger("MeleeAttack");
        }
        else if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
        {
            StartCoroutine(ActivateHitboxTemporarily(_hitBoxRight));
            //_animator.SetInteger("Direction", 3);
            //_animator.SetTrigger("MeleeAttack");
        }

        _nextAttackTime = Time.time + _attackCooldown;
    }



private IEnumerator ActivateHitboxTemporarily(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        hitbox.SetActive(false);
    }

}
