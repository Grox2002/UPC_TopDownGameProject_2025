using UnityEngine;
using System.Collections;

public class P_Attack : MonoBehaviour
{
    //Variables
    [SerializeField] private ShootController _shootController;
    [SerializeField] private GameObject _bow;

    [SerializeField] private MeleeController _meleeController;
    [SerializeField] private Parry _parry;

    private bool isMeleeAttacking = false;

    public Texture2D cursorTexture;

    [SerializeField] private Vector2 _hotspot = Vector2.zero;

    //Metodos

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, _hotspot, CursorMode.Auto);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !isMeleeAttacking)
        {
            StartCoroutine(ActivateMeleeAttack());
        }

        if (Input.GetMouseButtonDown(0) && _bow.activeInHierarchy && !isMeleeAttacking)
        {
            _shootController.Shoot();
        }
        if (_bow.activeInHierarchy && !isMeleeAttacking)
        {
            _shootController.RotateBowTowardsMouse();
        }
        if (Input.GetMouseButtonDown(1))
        {
            _parry.ActiveParry();
        }
       
    }

    private IEnumerator ActivateMeleeAttack()
    {
        isMeleeAttacking = true;
        _bow.SetActive(false);
        _meleeController.MeleeAttack();

        yield return new WaitForSeconds(0.3f);
        _bow.SetActive(true);
        isMeleeAttacking = false;
    }

}






