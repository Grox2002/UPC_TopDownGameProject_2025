using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostageSystem : MonoBehaviour
{
    //public static HostageSystem Instance { get; private set; }

    public TMP_Text hostagesToRescue;
    private int _totalHostages;
    private int _saved;
    public bool _allHostagesSaved = false;


    private void Start()
    {
        Hostage[] hostages = FindObjectsByType<Hostage>(FindObjectsSortMode.None);

        _totalHostages = hostages.Length;
        _saved = 0;

        foreach (var hostage in hostages)
        {
            if (PlayerPrefs.GetInt(hostage.hostageId, 0) == 1)
            {
                _saved++;
            }
        }

        UpdateUI();
    }

    public void Savaged()
    {
        _saved++;
        UpdateUI();


        if (_saved >= _totalHostages)
        {
            Debug.Log("¡Todos los rehenes han sido salvados!");
            MissionManager.Instance.CompleteCryptaHostages();
            
        }

    }

    private void UpdateUI()
    {
        hostagesToRescue.text = $"Jesuitas salvados: {_saved} / {_totalHostages}";
    }
}
