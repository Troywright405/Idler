using UnityEngine;

public class CharacterStats
{
    public string characterName;
    public PlayerClass characterClass;

    public Experience Experience { get; protected set; }

    protected virtual void Awake()
    {
        // PlayerStats needs this due to inherit
    }

    protected virtual void HandleLevelUp()
    {

    }
}
