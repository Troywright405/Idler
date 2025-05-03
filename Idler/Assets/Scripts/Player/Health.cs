using System;
using UnityEngine;

[System.Serializable]
public class Health
{
    public int _MaxHealth;
    public int Current;
    public float Regen;

    public int MaxHealth
    {
        get { return _MaxHealth; }
        set
        {
            // Only update if the value changes
            if (_MaxHealth != value)
            {
                _MaxHealth = value;
                UpdateRegen(); // Recalculate Regen whenever MaxHealth is updated
            }
        }
    }

    // Optional event for health changes
    public delegate void HealthChangedHandler();
    public event HealthChangedHandler OnHealthChanged;

    public Health(int max, float regen)
    {
        _MaxHealth = max;
        Current = max;
        Regen = Mathf.FloorToInt(_MaxHealth * 0.01f); //1% hp regen
    }

    public void TakeDamage(int amount)
    {
        Current = Mathf.Max(Current - amount, 0);
        OnHealthChanged?.Invoke();  // Notify listeners
    }

    public void Heal(int amount)
    {
        Current = Mathf.Min(Current + amount, _MaxHealth);
        OnHealthChanged?.Invoke();  // Notify listeners
    }
    public void Regenerate()
    {
        if (Regen > 0 && Current < _MaxHealth)
        {
            Heal(Convert.ToInt32(Regen));
        }
    }
    public void UpdateRegen()// Update the regeneration value when MaxHealth changes
    {
        Regen = Mathf.FloorToInt(_MaxHealth * 0.01f);
    }
    public void ResetHealth()
    {
        Current = _MaxHealth;
        OnHealthChanged?.Invoke();  // Notify listeners
    }
    public float Percentage => (float)Current / _MaxHealth;
}
