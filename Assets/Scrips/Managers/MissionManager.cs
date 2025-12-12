using UnityEngine;
using UnityEngine.SceneManagement;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;
    public bool CryptaHostagesCompleted { get; private set; }


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (!PlayerPrefs.HasKey("Game_Initialized"))
        {
            PlayerPrefs.SetInt("Mission_CriptaHostages", 0); 
            PlayerPrefs.SetInt("Game_Initialized", 1);        
            PlayerPrefs.Save();
        }


        Load();
    }

   


    public void CompleteCryptaHostages()
    {
        if (CryptaHostagesCompleted) return;

        CryptaHostagesCompleted = true;
        Save();
    }

    public void ResetCryptaHostages()
    {
        CryptaHostagesCompleted = false;
        Save();
    }

    private void Save() 
    {
        PlayerPrefs.SetInt("Mission_CriptaHostages", CryptaHostagesCompleted ? 1 : 0);
    }

    private void Load()
    {
        CryptaHostagesCompleted = PlayerPrefs.GetInt("Mission_CriptaHostages", 0) == 1;
    }

}
