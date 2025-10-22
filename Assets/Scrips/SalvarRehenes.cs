using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SalvarRehenes : MonoBehaviour
{
    public static SalvarRehenes Instance;

    public TMP_Text textoUI;
    private int totalRehenes;
    private int salvados;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalRehenes = FindObjectsOfType<Rehen>().Length;
        ActualizarUI();
    }

    public void RehenSalvado()
    {
        salvados++;
        ActualizarUI();

        if (salvados >= totalRehenes)
        {
            Debug.Log("¡Todos los rehenes han sido salvados!");
        }
    }

    private void ActualizarUI()
    {
        textoUI.text = $"Rehenes: {salvados} / {totalRehenes}";
    }
}
