using UnityEngine;

public abstract class CharacterStats : MonoBehaviour
{
    public string characterName;
    public PlayerClass characterClass;

    public Health Health { get; protected set; }
    public Experience Experience { get; protected set; }

    protected virtual void Awake()
    {
        Health = new Health(100, 1); //shouldn't be hard coded
        if (Experience == null) Experience = new Experience();    
    }


    protected abstract void HandleLevelUp(int newLevel, CombatStatStructure newStats);
}
