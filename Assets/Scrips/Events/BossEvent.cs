using System.Collections;
using TMPro.Examples;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;



public class BossEvent : MonoBehaviour
{
    [SerializeField] private SeraphBoss _boss;
    [SerializeField] private Transform _bossTransform;
    [SerializeField] private DialogSO _FirstEncounterBossDialog;
    [SerializeField] private DialogSO _defeatedBossDialog;
    [SerializeField] private CinemachineVirtualCameraBase _virtualCamera;
    [SerializeField] private float _smooth;
    [SerializeField] private Transform _targetPoint;
    [SerializeField] private GameObject _seraph;
    [SerializeField] private GameObject _closedDoor;
    [SerializeField] private GameObject _decorations;
    [SerializeField] private Animator _transitionAnimator;

    private Collider2D _eventTrigger;

    private bool triggered = false;

    [SerializeField] private AudioSource _victoryAnthem;

    void Awake()
    {
        _eventTrigger = GetComponent<Collider2D>();

        _eventTrigger.enabled = false;

        if (PlayerPrefs.GetInt("Mission_CriptaHostages", 0) == 1)
        {
            _eventTrigger.enabled = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            triggered = true;
            StartCoroutine(BossIntroSequence());
        }
    }

    private IEnumerator BossIntroSequence()
    {
        _seraph.SetActive(true);
        GameManager.Instance.playerMovement.canMove = false;
        GameManager.Instance.playerAttack.canAttack = false;
        _decorations.SetActive(false);

        if (_seraph.activeSelf)
        _virtualCamera.Follow = _bossTransform;
        _virtualCamera.LookAt = _bossTransform;

        while (Vector2.Distance(_bossTransform.position, _targetPoint.position) > 0.05f)
        {
            _bossTransform.position = Vector2.MoveTowards(_bossTransform.position, _targetPoint.position, _smooth * Time.deltaTime);

            yield return null;
        }
        _bossTransform.position = _targetPoint.position;

        yield return new WaitForSeconds(1f);

        DialogManager.Instance.StartDialogue(_FirstEncounterBossDialog);

        yield return StartCoroutine(DialogManager.Instance.WaitForDialogue());

        _virtualCamera.Follow = GameManager.Instance.playerTransform;
        _virtualCamera.LookAt = GameManager.Instance.playerTransform;

        _boss.StartBattle();

        GameManager.Instance.playerMovement.canMove = true;
        GameManager.Instance.playerAttack.canAttack = true;
        _closedDoor.SetActive(true);

        yield return new WaitForSeconds(1f);

        _eventTrigger.enabled = false;

    }

    public void BossIsDefeated()
    {
        StartCoroutine(BossDefeatSequence());
    }
    public IEnumerator BossDefeatSequence()
    {
        _virtualCamera.Follow = _bossTransform;
        _virtualCamera.LookAt = _bossTransform;

        yield return new WaitForSeconds(1f);

        _victoryAnthem.Play();

        DialogManager.Instance.StartDialogue(_defeatedBossDialog);

        yield return new WaitUntil(() => DialogManager.Instance.IsDialogueActive);

        yield return new WaitUntil(() => !DialogManager.Instance.IsDialogueActive);

        yield return StartCoroutine(BossAscendAnimation());

        _virtualCamera.Follow = GameManager.Instance.playerTransform;
        _virtualCamera.LookAt = GameManager.Instance.playerTransform;

        yield return new WaitForSeconds(1f);

        _transitionAnimator.SetTrigger("StartFadeOut");
        yield return new WaitForSeconds(2f);

        GameManager.Instance.BossDefeated();

        Destroy(gameObject);

    }

    private IEnumerator BossAscendAnimation()
    {
        float duration = 4f;
        float timer = 0f;
        float speed = 3f;

        while (timer < duration)
        {
            _bossTransform.position += Vector3.up * speed * Time.deltaTime;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
