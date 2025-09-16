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
    //
    /*
    
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _meleeAttackRange = 0.5f;
    [SerializeField] private int _meleeDamage;
   

    private float _nextAttack;
    */

    private void Start()
    {
        DisableAllHitboxes();
        Debug.Log("Hitboxes desactivados al inicio");
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
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.E))
        {
            _hitBoxUp.SetActive(true);
            _animator.SetInteger("Direction", 0);
            _animator.SetTrigger("MeleeAttack");
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.E))
        {
            _hitBoxDown.SetActive(true);
            _animator.SetInteger("Direction", 1);
            _animator.SetTrigger("MeleeAttack");
        }
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.E))
        {
            _hitBoxLeft.SetActive(true);
            _animator.SetInteger("Direction", 2);
            _animator.SetTrigger("MeleeAttack");
        }
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.E))
        {
            _hitBoxRight.SetActive(true);
            _animator.SetInteger("Direction", 3);
            _animator.SetTrigger("MeleeAttack");
        }
    }

}
