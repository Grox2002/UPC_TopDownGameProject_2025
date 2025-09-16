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
        if (Input.GetMouseButtonDown(0))
        {
            if (_shotController != null)
            {
                _shotController.Shoot();
            }
        }


        // Ataque cuerpo a cuerpo
        if (Input.GetKeyDown(KeyCode.E)) //Aqui ira la logica de input mas adelante
        {
            //_parry.ActivateParry();

            //_meleeController.MeleeAttack();

        }

    }
    
}






