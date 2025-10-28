using System.Collections;
using TMPro.Examples;
using Unity.Cinemachine;
using UnityEngine;



public class BossEvent : MonoBehaviour
{
    [SerializeField] private SeraphBoss _boss;
    [SerializeField] private Transform _bossFocus;
    [SerializeField] private DialogSO _bossDialog;
    [SerializeField] private DialogManager _dialogManager;
    [SerializeField] private CinemachineVirtualCameraBase _virtualCamera;
    [SerializeField] private Transform _playerFocus;

    [SerializeField] private P_Attack p_Attack;
    [SerializeField] private P_Movement p_Movement;


    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.CompareTag("Player"))
        {
            p_Attack.enabled = false;
            p_Movement.enabled = false;

            triggered = true;
            StartCoroutine(BossIntroSequence());


        }
    }

    private IEnumerator BossIntroSequence()
    {
        
        //p_Attack.enabled = false;
        //p_Movement.enabled = false;

        _virtualCamera.Follow = _bossFocus;
        _virtualCamera.LookAt = _bossFocus;

        yield return new WaitForSeconds(1f);

        _dialogManager.StartDialogue(_bossDialog);

        yield return new WaitUntil(() => _dialogManager.dialogueBox.activeSelf == false);

        _virtualCamera.Follow = _playerFocus;
        _virtualCamera.LookAt = _playerFocus;

        _boss.StartBattle();

        
    }
}
