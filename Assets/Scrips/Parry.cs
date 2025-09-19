using UnityEngine;
using System.Collections;

public class Parry : MonoBehaviour //ESTO ESTA FASE EXPERIMENTAL
{
    [Header("Attack Points")]
    [SerializeField] private GameObject _parryUp;
    [SerializeField] private GameObject _parryDown;
    [SerializeField] private GameObject _parryLeft;
    [SerializeField] private GameObject _parryRight;

    [SerializeField] private float _parryDuration = 0.2f;
    public void ActiveParry()
    {

        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 ParryDirection = (mouseWorldPos - transform.position).normalized;

        if (Mathf.Abs(ParryDirection.y) > Mathf.Abs(ParryDirection.x))
        {
            if (ParryDirection.y > 0)
                StartCoroutine(ActivateParryHitbox(_parryUp));
            else
                StartCoroutine(ActivateParryHitbox(_parryDown));
        }
        else
        {
            if (ParryDirection.x > 0)
                StartCoroutine(ActivateParryHitbox(_parryRight));
            else
                StartCoroutine(ActivateParryHitbox(_parryLeft));
        }


    }

    private IEnumerator ActivateParryHitbox(GameObject hitbox)
    {
        DisableAllHitboxes();
        hitbox.SetActive(true);
        yield return new WaitForSeconds(_parryDuration);
        hitbox.SetActive(false);
    }

    private void DisableAllHitboxes()
    {
        _parryUp.SetActive(false);
        _parryDown.SetActive(false);
        _parryLeft.SetActive(false);
        _parryRight.SetActive(false);
    }


    /*
    private void Start()
    {
        DisableAllHitboxes();

    }

    private void Update()
    {
        ActiveParry();
    }


    void DisableAllHitboxes()
    {
        _parryUp.SetActive(false);
        _parryLeft.SetActive(false);
        _parryDown.SetActive(false);
        _parryRigth.SetActive(false);
    }


    public void ActiveParry()
    {

        if (Input.GetKey(KeyCode.W) && (Input.GetMouseButtonDown(1)))
        {
            StartCoroutine(ParryWindow(_parryUp));
            
        }
        else if (Input.GetKey(KeyCode.S) && (Input.GetMouseButtonDown(1)))
        {
            StartCoroutine(ParryWindow(_parryDown));
            
        }
        else if (Input.GetKey(KeyCode.A) && (Input.GetMouseButtonDown(1)))
        {
            StartCoroutine(ParryWindow(_parryLeft));
            
        }
        else if (Input.GetKey(KeyCode.D) && (Input.GetMouseButtonDown(1)))
        {
            StartCoroutine(ParryWindow(_parryRigth));
            
        }

    }


    private IEnumerator ParryWindow(GameObject hitbox)
    {
        hitbox.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        hitbox.SetActive(false);
    }
    */
}


