using UnityEngine;
using System.Collections;

public class Parry : MonoBehaviour
{
    [Header("Parry")]
    [SerializeField] private Collider2D _parryUp;
    [SerializeField] private Collider2D _parryDown;
    [SerializeField] private Collider2D _parryRight;
    [SerializeField] private Collider2D _parryLeft;

    //[SerializeField] private Collider2D parryCollider;
    [SerializeField] private float parryDuration = 0.3f;

    void Awake()
    {
        DeactivateAllParryColliders(); 
    }


    public void ActivateParry()
    {
        StartCoroutine(ParryWindow());
    }

    private void ActiveAllParryColliders()
    {
        _parryUp.enabled = true;
        _parryDown.enabled = true;
        _parryRight.enabled = true;
        _parryLeft.enabled = true;
    }
    private void DeactivateAllParryColliders()
    {
        _parryUp.enabled = false;
        _parryDown.enabled = false;
        _parryRight.enabled = false;
        _parryLeft.enabled = false;
    }

    private IEnumerator ParryWindow()
    {
        //parryCollider.enabled = true;
        ActiveAllParryColliders();
        yield return new WaitForSeconds(parryDuration);
        DeactivateAllParryColliders();
        //parryCollider.enabled = false;
    }

}


