using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HeartUI : MonoBehaviour
{
    public static HeartUI Instance;

    public GameObject heartPrefab;    
    public Sprite heartFull;
    public Sprite heartHalf;
    public Sprite heartEmpty;

    public int healthPerHeart = 10;

    private List<Image> hearts = new List<Image>();

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        P_Health.OnHealthChanged += Refresh;
    }

    private void OnDisable()
    {
        P_Health.OnHealthChanged -= Refresh;
    }


    // Actualiza el HUD de corazones según la vida y vida máxima.
    public void Refresh(float currentHealth, float maxHealth)
    {
        int totalHeartsRequired = Mathf.CeilToInt(maxHealth / healthPerHeart);

        // Crear los corazones si faltan
        while (hearts.Count < totalHeartsRequired)
        {
            GameObject heart = Instantiate(heartPrefab, transform);
            Image img = heart.GetComponent<Image>();
            hearts.Add(img);
        }

        // Activar solo los necesarios
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].gameObject.SetActive(i < totalHeartsRequired);
        }

        // Asignar sprites según vida
        for (int i = 0; i < totalHeartsRequired; i++)
        {
            float value = currentHealth - (i * healthPerHeart);

            hearts[i].sprite =
                value >= healthPerHeart ? heartFull :
                value > 0 ? heartHalf :
                heartEmpty;
        }
    }

}
