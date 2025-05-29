using UnityEngine;
using System;

public class Experience
{
    public Action<float> OnXPChanged;
    public Action<int> OnLevelChanged;
    public int Xp { get; private set; }  // Current XP
    public int Level { get; private set; } = 1;  // Current level
    private const int BaseXP = 100;  // Starting XP for level 1
    public float ExperiencePercentage => (XPToLevel(Level) == 0) ? 0f : (float)Xp / XPToLevel(Level);
    public float Normalized => (Xp > 0 && XPToLevel(Level) > 0) ? Mathf.Clamp01((float)Xp / XPToLevel(Level)) : 0f;
    public Experience(int level, int xp = 0)
    {
        Level = Mathf.Max(1, level);
        Xp = xp;
        OnXPChanged?.Invoke(ExperiencePercentage); // Trigger the initial UI update on creation of Experience object
        OnLevelChanged?.Invoke(Level);
    }
    /// <summary>
    /// Adds XP and returns true if level changed.
    /// </summary>
    public bool AddXP(int amount)
    {
        Xp += amount;
        OnXPChanged?.Invoke(Normalized);
        return CheckForLevelUp();
    }


    private bool CheckForLevelUp()
    {
        int priorLevel = Level;
        while (Xp >= XPToLevel(Level))
        {
            Xp -= XPToLevel(Level);
            Level++;
            OnLevelChanged?.Invoke(Level);
            OnXPChanged?.Invoke(Normalized); // after level-up the bar resets
        }

        return priorLevel != Level;
    }


    public int XPToLevel(int level)
    {
        return (int)(BaseXP * Mathf.Pow(1.2f, level - 1));
    }

}
