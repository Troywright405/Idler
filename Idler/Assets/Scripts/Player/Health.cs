using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public class Health
{
    private int _MaxHealth=1;
    public float Current;
    public float RegenAmount; //Integer-compatible version, still float due to not yet processing rounding maths
    public float Regen;
    public int totalDamageTaken=0; //dumb stat but we can keep for now

    public delegate void HealthChangedHandler();
    public event HealthChangedHandler OnHealthChanged;
    public int MaxHealth
    {
        get { return _MaxHealth; }
        set
        {
            _MaxHealth = value;
            CalculateRegenAmount(Regen); //auto recalculates regen amount if maxhealth is changed
        }
    }
    public Health(int max, float regenRate = 0) //Accepts optional and uses 0 as placeholder because health objects are used before the character is fully initialized
    {
        _MaxHealth = Mathf.Max(1, max); //Prevents it from initializing with 0 or less hp
        Current = max;
        Regen = regenRate; //globalizes regenRate for use in auto recalc from max health changing via "set" above
        CalculateRegenAmount(regenRate); // Converts % based to an actual amount healed, similar to int but still stored as float
    }

    public void TakeDamage(int amount)
    {
        Current = Mathf.Max(Current - amount, 0);
        totalDamageTaken += amount;
        OnHealthChanged?.Invoke();  // Notify listeners
    }

    public void Heal(int amount)
    {
        Current = Mathf.Min(Current + amount, _MaxHealth);
        OnHealthChanged?.Invoke();  // Notify listeners
    }
    public void Regenerate()
    {
        if (RegenAmount > 1 && Current < _MaxHealth)
        {
            Heal(Convert.ToInt32(RegenAmount));
        }
        else if (RegenAmount > 0 && RegenAmount <= 1)
        {
            Heal(1); // Minimum heal is 1 as long as there is any heal at all
        }
        else if (RegenAmount == 0f)
        {
            // Idk, shouldn't ever be 0. Maybe later this area can do something? If regen debuffs, better to control via flags or new params than this
        }
        else
        {
            Current = _MaxHealth;
        }
    }
    private void CalculateRegenAmount(float regenRate)
    {
        RegenAmount = _MaxHealth * regenRate; // Gets the non-rounded float version of max health times regen percentage from base stats
    }
    public void ResetHealth()
    {
        Current = _MaxHealth;
        OnHealthChanged?.Invoke();  // Notify listeners
    }
    public float Percentage => Current / _MaxHealth;
    public float PercentageRegen => Regen * 100; //Float value to whole % number
}
