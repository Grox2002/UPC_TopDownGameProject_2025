using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsPlayerDead { get; private set; }

    [Header("Player References")]
    public GameObject player;
    public Rigidbody2D playerRb;
    public Collider2D playerCollider;
    public P_Health playerHealth;
    public P_Movement playerMovement;
    public P_Attack playerAttack;
    public Transform playerTransform;
    public GameObject shootController;
    public Vector3 initialPlayerPosition;

    public TimerSystem globalTimer;

    [SerializeField] private AudioSource _deathBells;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }


    //=============Testing=============//
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UI_Manager.Instance.ShowPauseMenu();
        }

        //============Only Testing===============// //Por favor mantener esto comentado antes de generar la build
        //if (Input.GetKeyDown(KeyCode.T))
            //MissionManager.Instance.CompleteCryptaHostages();
        //=======================================//
    }
    
    

    public void RegisterPlayer(GameObject PlayerGameObject)
    {
        player = PlayerGameObject;
        playerTransform = PlayerGameObject.transform;
        playerHealth = PlayerGameObject.GetComponent<P_Health>();
        playerMovement = PlayerGameObject.GetComponent<P_Movement>();
        playerAttack = PlayerGameObject.GetComponent<P_Attack>();
    }

    public void PlayerDied()
    {
        IsPlayerDead = true;
        _deathBells.Play();
        PlayerStats.Instance.ResetPoints();
    }
    public void PlayerAlive()
    {
        IsPlayerDead = false;
    }

    public void BossDefeated()
    {
        MissionManager.Instance.ResetCryptaHostages();
        SceneManager.LoadScene("VictoryScene");
        playerHealth.ResetHealthToFull(); 
    }

    public void GameOver()
    {
        IsPlayerDead = true;
        UI_Manager.Instance.ShowDefeatMenu();
    }

    
}
