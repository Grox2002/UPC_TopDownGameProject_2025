using System.Collections;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    public string barrelId = "Barrel_01";

    [SerializeField] private float _life = 50;
    [SerializeField] private AudioSource _barrelImpact;
    [SerializeField] private AudioSource _barrelDestroyed;
    [SerializeField] private GameObject _pancito;

    private Collider2D _collider;
    private Animator _animator;


    void Awake()
    {
        if (PlayerPrefs.GetInt(barrelId, 0) == 1)
        {
            gameObject.SetActive(false);
        }
    }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider = GetComponent<Collider2D>();
    }

    public void TakeDamage(float damage)
    {
        _life -= damage;

        if (_life <= 0)
        {
            Instantiate(_pancito, transform.position, Quaternion.identity);
            StartCoroutine(Destroy());
        }
        else
        {
            _barrelImpact.Play();
        }
    }

    private IEnumerator Destroy()
    {
        PlayerPrefs.SetInt(barrelId, 1);
        PlayerPrefs.Save();

        _collider.enabled = false;
        _animator.SetTrigger("Destroy");
        _barrelDestroyed.Play();

        yield return new WaitForSeconds(0.7f);

        Destroy(gameObject);
    }
}
