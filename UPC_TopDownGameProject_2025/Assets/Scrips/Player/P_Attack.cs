using UnityEngine;


public class P_Attack : MonoBehaviour
{
    //Variables
    [SerializeField] private ShootController _shotController;
    [SerializeField] private Parry _parry;
    [SerializeField] private MeleeController _meleeController;

    //Metodos
    void Update()
    {
        //Disparo
        if (Input.GetKey(KeyCode.Space))
        {
            if (_shotController != null)
            {
                _shotController.Shoot();
            }
        }


        // Ataque cuerpo a cuerpo
        if (Input.GetKeyDown(KeyCode.E))
        {
            _parry.ActivateParry();

            //_meleeController.MeleeAttack();

        }

    }
    
}






