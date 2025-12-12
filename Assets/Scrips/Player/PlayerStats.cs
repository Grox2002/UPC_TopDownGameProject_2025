using System;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    public event Action<int> OnPointsChanged;

    private int _points = 0;
    public int points
    {
        get => _points;
        private set
        {
            if (_points == value) return; 
            _points = value;
            OnPointsChanged?.Invoke(_points);
        }
    }

    public float maxHealth = 10f;
    public float shootDamage = 8f;
    public float meleeDamage = 10f;
    public float maxStamina = 20f;
    public float staminaRecovery = 5f;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

    }

    public void ResetPoints()
    {
        SetPoints(0);
    }

    // sumar puntos
    public void AddPoints(int amount)
    {
        if (amount == 0) return;
        SetPoints(Mathf.Max(0, _points + amount));
    }

    // setear puntos directamente 
    public void SetPoints(int value)
    {
        int clamped = Mathf.Max(0, value);
        points = clamped;
    }


    public void AddMaxHealth(int amount)
    {
        float oldMax = maxHealth;

        maxHealth = Mathf.Min(maxHealth + amount, 50f);

        // Buscar al jugador
        var health = FindFirstObjectByType<P_Health>();

        if (health != null)
        {
            // Curar SOLO la diferencia entre el nuevo max y el anterior
            float healthIncrease = maxHealth - oldMax;
            if (healthIncrease > 0f)
                health.Heal(healthIncrease);
        }
    }

    public void ResetAllStats()
    {
        maxHealth = 10f;
        maxStamina = 20f;
        shootDamage = 8f;
        meleeDamage = 10f;
    }
}

