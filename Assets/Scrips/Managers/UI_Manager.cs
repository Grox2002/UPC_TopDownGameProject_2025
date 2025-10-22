using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    public bool _gameOver = false;

    [Header("HUD")]
    [SerializeField] private Image _playerHealthBar;
    [SerializeField] private Image _staminaBar;

    [SerializeField] private Image _bossHealthBar;
    [SerializeField] private GameObject _bossHUD;

    [Header("Menu Type")]
    [SerializeField] private GameObject _defeatMenu;
    [SerializeField] private GameObject _pauseMenu;
    

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowPauseMenu();
        }
    }

    //Update player health bar
    public void UpdatePlayerHealthBar(float current, float max)
    {
        _playerHealthBar.fillAmount = current / max;
    }

    //Update boss health bar
    public void UpdateBossHealthBar(int current, int max)
    {
        _bossHealthBar.fillAmount = (float)current / max;
    }

    public void HideBossHealthBar()
    {
        _bossHUD.SetActive(false);
    }

    //Update Stamina bar
    public void UpdateStaminaBar(float current, float max)
    {
        _staminaBar.fillAmount = current / max;
    }



    //Defeat Menu
    public void ShowDefeatMenu()
    {
        if (_defeatMenu != null)
        {
            _gameOver = true;
            _defeatMenu.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void CloseDefeatMenu()
    {
        Time.timeScale = 1f;
        _defeatMenu.SetActive(false);
    }

    //Interactive Menu
    public void ShowPauseMenu()
    {
        Time.timeScale = 0f;
        _pauseMenu.SetActive(true);
    }
    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
    }

   
    //------------------------------BUTONS---------------------------------------
    public void Restart()
    {
        SceneLoaderManager.Instance.LoadActiveScene();
        CloseDefeatMenu();
    }

    public void ReturnMainMenu()
    {
        SceneLoaderManager.Instance.LoadMainMenuScene();
        ClosePauseMenu();
        CloseDefeatMenu();
    }

    public void Continue()
    {
        ClosePauseMenu();
    }

}
