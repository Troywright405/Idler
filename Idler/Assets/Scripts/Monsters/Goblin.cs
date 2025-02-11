using System;

public class Goblin : Monster
{

    public Goblin()
    {
        maxHealth = 50;
        nameOfSpecies = "Goblin";
        attackPowerMelee = 10;
        attackPowerRanged = 5;
        attackPowerMagic = 2;
        defenseMelee = 8;
        defenseRanged = 4;
        defenseMagic = 2;
        hitRateMelee = 0.75f;
        hitRateRanged = 0.65f;
        hitRateMagic = 0.50f;
        evasionMelee = 0.20f;
        evasionRanged = 0.15f;
        evasionMagic = 0.10f;
        attackSpeed = 1.2f;
        movementSpeed = 1.5f;
        experienceReward = 10;
        courage = 5;
        description = "A small but cunning creature that relies on numbers and trickery to win fights.";
    }
    public override void TakeDamage(int damage, string nameOfSpecies = "???")
    {
        base.TakeDamage(damage,nameOfSpecies);
    }
}
