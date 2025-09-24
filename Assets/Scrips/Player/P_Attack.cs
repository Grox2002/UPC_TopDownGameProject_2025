using UnityEngine;
using System.Collections;

public class P_Attack : MonoBehaviour
{
    //Variables
    [SerializeField] private ShootController _shootController;
    [SerializeField] private GameObject _bow;

    [SerializeField] private MeleeController _meleeController;
    [SerializeField] private Parry _parry;

    public bool isMeleeAttacking = false;

    public Texture2D cursorTexture;

    [SerializeField] private Vector2 _hotspot = Vector2.zero;

    //estamina
    [SerializeField] private float _maxStamina = 8f;
    [SerializeField] private float _staminaRecoveryRate = 1f;
    [SerializeField] private float _meleeStaminaCost = 1f;
    [SerializeField] private float _shootStaminaCost = 0.5f;

    private float _currentStamina;

    [SerializeField] private float _staminaRecoveryDelay = 1f; // segundos de espera
    private float _lastStaminaUseTime;




    //Metodos
    private void Start()
    {
        Cursor.SetCursor(cursorTexture, _hotspot, CursorMode.Auto);
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        // Recuperación de estamina
        if (_currentStamina < _maxStamina && Time.time >= _lastStaminaUseTime + _staminaRecoveryDelay)
        {
            _currentStamina += _staminaRecoveryRate * Time.deltaTime;
            _currentStamina = Mathf.Min(_currentStamina, _maxStamina);
        }

        UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);

        if (Input.GetKeyDown(KeyCode.E) && !isMeleeAttacking && _currentStamina >= _meleeStaminaCost)
        {
            _currentStamina -= _meleeStaminaCost; // Descontar antes
            _lastStaminaUseTime = Time.time;
            //UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
            StartCoroutine(ActivateMeleeAttack());
        }

        if (Input.GetMouseButtonDown(0) && _bow.activeInHierarchy && !isMeleeAttacking && _currentStamina >= _shootStaminaCost)
        {
            _currentStamina -= _shootStaminaCost;
            _lastStaminaUseTime = Time.time;
            _shootController.Shoot();
            //UI_Manager.Instance.UpdateStaminaBar(_currentStamina, _maxStamina);
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

    public IEnumerator ActivateMeleeAttack()
    {
        isMeleeAttacking = true;

        _bow.SetActive(false);
        _meleeController.MeleeAttack();

        yield return new WaitForSeconds(0.2f);

        _bow.SetActive(true);
        isMeleeAttacking = false;
    }

}






