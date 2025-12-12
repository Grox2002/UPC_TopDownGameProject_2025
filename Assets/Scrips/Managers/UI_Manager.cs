using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class UI_Manager : MonoBehaviour
{
    public static UI_Manager Instance;
    public bool _gameOver = false;

    [Header("Player HUD")]
    [SerializeField] private TMP_Text _pointsTextOnScreen;
    [SerializeField] private Image _staminaBar;

    [Header("Menu")]
    [SerializeField] private GameObject _defeatMenu;
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _statsMenu;


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
            ShowPauseMenu();

        
        if (Input.GetKeyDown(KeyCode.U) && !_statsMenu.activeInHierarchy || _defeatMenu.activeInHierarchy)
            OpenStatsMenu();

        else if(Input.GetKeyDown(KeyCode.U) && _statsMenu.activeInHierarchy)
            CloseStatsMenu();

        UpdateUIPoints();


    }

    private void UpdateUIPoints()
    {
        _pointsTextOnScreen.text = "Faith Points: " + PlayerStats.Instance.points;
    }

    private void OpenStatsMenu()
    {
        if (_pauseMenu.activeInHierarchy) return;
        if (_defeatMenu.activeInHierarchy) return;
        _statsMenu.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0f;
    }
    private void CloseStatsMenu()
    {
        _statsMenu.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1f;
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
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }
    

    public void CloseDefeatMenu()
    {
        Time.timeScale = 1f;
        _defeatMenu.SetActive(false);
        Cursor.visible = false;
    }

    //Interactive Menu
    public void ShowPauseMenu()
    {
        if (_defeatMenu.activeInHierarchy) return;
        if (_statsMenu.activeInHierarchy) return;
        _pauseMenu.SetActive(true);
        Cursor.visible = true;
        if (_pauseMenu.activeSelf)
            Time.timeScale = 0f;
    }
    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        _pauseMenu.SetActive(false);
        Cursor.visible = false;

    }

   
    //------------------------------BUTONS---------------------------------------
    public void Restart()
    {
        SceneLoaderManager.Instance.LoadActiveScene();
        CloseDefeatMenu();
        ClosePauseMenu();
        
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
