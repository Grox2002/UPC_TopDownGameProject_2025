using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
            UI_Manager.Instance.ShowPauseMenu();
        }
    }

    public void BossDefeated()
    {
        SceneManager.LoadScene("VictoryScene");
    }

    public void GameOver()
    {
        UI_Manager.Instance.ShowDefeatMenu();
    }

}
