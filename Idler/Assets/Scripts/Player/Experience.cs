using UnityEngine;

public class Experience
{
    public float ExperiencePercentage => (XPToLevel(Level) == 0) ? 0f : (float)Xp / XPToLevel(Level);

    public int Xp {get; private set;}  // Current XP
    public int Level { get; private set; } = 1;  // Current level
    private const int BaseXP = 100;  // Starting XP for level 1

    /// <summary>
    /// Adds XP and returns true if level changed.
    /// </summary>
    public bool AddXP(int amount)
    {
        Xp += amount;
        return CheckForLevelUp();
    }


    private bool CheckForLevelUp()
    {
        bool leveledUp = false;

        while (Xp >= XPToLevel(Level))
        {   
        Xp -= XPToLevel(Level);
            Level++;
            leveledUp = true;
        }

        return leveledUp;
    }


    public int XPToLevel(int level)
    {
        return (int)(BaseXP * Mathf.Pow(1.2f, level - 1));
    }
}
