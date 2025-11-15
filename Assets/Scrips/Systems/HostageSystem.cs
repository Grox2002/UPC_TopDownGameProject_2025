using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HostageSystem : MonoBehaviour
{
    public static HostageSystem Instance;

    public TMP_Text textUI;
    private int totalHostages;
    private int save;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalHostages = FindObjectsByType<Hostage>(FindObjectsSortMode.None).Length;
        ActualizarUI();
    }

    public void Savaged()
    {
        save++;
        ActualizarUI();

        if (save >= totalHostages)
        {
            Debug.Log("¡Todos los rehenes han sido salvados!");
        }
    }

    private void ActualizarUI()
    {
        textUI.text = $"Rehenes: {save} / {totalHostages}";
    }
}
