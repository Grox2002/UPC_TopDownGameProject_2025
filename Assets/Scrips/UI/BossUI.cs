using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BossUI : MonoBehaviour
{
    public static BossUI Instance { get; private set; }

    [Header("Boss HUD")]
    [SerializeField] private Image _bossHealthBar;
    [SerializeField] private Image _invulnerableBar;
    [SerializeField] private GameObject _bossInvulnerablehBar;
    [SerializeField] private GameObject _bossHUD;
    [SerializeField] private GameObject _bossName;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _bossHUD.SetActive(false);
    }


    //=============== Update Bars =======================================//
    public void UpdateBossHealthBar(float current, float max)
    {
        _bossHealthBar.fillAmount = current / max;
    }
    public void UpdateTransitionBar(float current, float max)
    {
        _invulnerableBar.fillAmount = current / max;
    }


    //============== Show & Hide Bars ==================================//
    public void HideTransitionBar()
    {
        _bossInvulnerablehBar.SetActive(false);
        _bossName.SetActive(true);
    }
    public void ShowTransitionBar()
    {
        _bossInvulnerablehBar.SetActive(true);
        _bossName.SetActive(false);
    }


    public void ShowBossHealthBar()
    {
        _bossHUD.SetActive(true);
    }
    public void HideBossHealthBar()
    {
        _bossHUD.SetActive(false);
    }
}
