using System;

public class Orc : Monster
{
    public Orc()
    {
        nameOfSpecies = "Orc";
        maxHealth = 250;
        attackPowerMelee = 50;
        attackPowerRanged = 25;
        attackPowerMagic = 5;
        defenseMelee = 40;
        defenseRanged = 30;
        defenseMagic = 10;
        hitRateMelee = 0.75f;
        hitRateRanged = 0.70f;
        hitRateMagic = 0.60f;
        evasionMelee = 0.30f;
        evasionRanged = 0.20f;
        evasionMagic = 0.10f;
        attackSpeed = 1.2f;
        movementSpeed = 1.8f;
        experienceReward = 100;
        courage = 60;
        description = "A large, brutish creature often found in swarms. Known for raw strength and relentless attacks.";
    }

    public override void TakeDamage(int damage, string nameOfSpecies = "???")
    {
        base.TakeDamage(damage,nameOfSpecies);
    }
}
