using UnityEngine;


public class E_Melee : MonoBehaviour
{
    //Variables
    [SerializeField] private int _damage;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private float _attackRange;
   
    private float _nextAttack;
   

    //Metodos
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Time.time >= _nextAttack)
        {
            var health = other.GetComponent<P_Health>();
            if (health != null)
            {
                health.TakeDamage(_damage);
                Debug.Log($"Player recibió {_damage} de daño");
            }

            _nextAttack = Time.time + _attackCooldown;
        }
    }



}
