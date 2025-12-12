using System;
using UnityEngine;

public class DroneEye : MonoBehaviour
{
    [Header("Attack Configuration")]
    [SerializeField] private float _attackInterval = 2f;
    [SerializeField] private int _damage = 10;
    private Animator _animator;
    private float timer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= _attackInterval)
        {
            _animator.SetTrigger("EyeAttack");
            timer = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<P_Health>().TakeDamage(_damage);
        }

    }

    
}
