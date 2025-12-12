using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatsUI : MonoBehaviour
{
    public static StatsUI Instance { get; private set; }

    [Header("Texts")]
    public TMP_Text pointsText;
    public TMP_Text healthText;
    public TMP_Text damageText;
    public TMP_Text meleeDamageText;
    public TMP_Text staminaText;

    [Header("Buttons")]
    public Button upgradeHealthButton;
    public Button upgradeDamageButton;
    public Button upgradeStaminaButton;
    public Button upgradeMeleeButton;

    [Header("Costs")]
    public int costHealth = 20;
    public int costDamage = 40;
    public int costStamina = 5;
    public int costMelee = 40;

    [Header("Upgrade Amount")]
    public int healthIncrease = 10;
    public int damageIncrease = 2;
    public int staminaIncrease = 5;
    public int meleeIncrease = 5;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();

        if(PlayerStats.Instance != null)
            PlayerStats.Instance.OnPointsChanged += OnPointsChanged;
    }

    private void OnDestroy()
    {
        
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.OnPointsChanged -= OnPointsChanged;
    }

    private void OnPointsChanged(int newPoints)
    {
        UpdateUI(); 
    }


    
    //=========================== UPGRADE METHODS ================================================//
    public void UpgradeHealth()
    {
        if (PlayerStats.Instance.maxHealth >= 50) return;

        if (PlayerStats.Instance.points >= costHealth)
        {
            PlayerStats.Instance.SetPoints(PlayerStats.Instance.points - costHealth);

            // Aumentar el máximo
            PlayerStats.Instance.AddMaxHealth(healthIncrease);

            // Curar para que aparezca el corazon nuevo
            var health = FindFirstObjectByType<P_Health>();
            if (health != null)
                health.Heal(healthIncrease);
        }

        UpdateUI();
    }

    public void UpgradeDamage()
    {
        if (PlayerStats.Instance.points >= costDamage)
        {
            PlayerStats.Instance.SetPoints(PlayerStats.Instance.points - costDamage);
            PlayerStats.Instance.shootDamage += damageIncrease;
        }

        UpdateUI();
    }

    public void UpgradeMelee()
    {
        if (PlayerStats.Instance.points >= costMelee)
        {
            PlayerStats.Instance.SetPoints(PlayerStats.Instance.points - costMelee);
            PlayerStats.Instance.meleeDamage += meleeIncrease;
        }

        UpdateUI();
    }

    public void UpgradeStamina()
    {
        if (PlayerStats.Instance.points >= costStamina)
        {
            PlayerStats.Instance.SetPoints(PlayerStats.Instance.points - costStamina);
            PlayerStats.Instance.maxStamina += staminaIncrease;
        }

        UpdateUI();
    }

    

    //================================================ UI UPDATE =====================================================//
    public void UpdateUI()
    {
        if (PlayerStats.Instance == null) return;

        pointsText.text = "Faith Points: " + PlayerStats.Instance.points;
        healthText.text = "Max Health: " + PlayerStats.Instance.maxHealth;
        damageText.text = "Shoot Damage: " + PlayerStats.Instance.shootDamage;
        staminaText.text = "Stamina: " + PlayerStats.Instance.maxStamina;
        meleeDamageText.text = "Melee Damage: " + PlayerStats.Instance.meleeDamage;

        // Activar o desactivar según puntos
        upgradeHealthButton.interactable =
            PlayerStats.Instance.points >= costHealth &&
            PlayerStats.Instance.maxHealth < 50;

        upgradeDamageButton.interactable =
            PlayerStats.Instance.points >= costDamage;

        upgradeStaminaButton.interactable =
            PlayerStats.Instance.points >= costStamina;

        upgradeMeleeButton.interactable =
            PlayerStats.Instance.points >= costMelee;
    }
}
