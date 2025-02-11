public class Slime : Monster
{
    public Slime()
    {
        nameOfSpecies = "Slime";
        maxHealth = 20;
        attackPowerMelee = 5;
        attackPowerRanged = 0;
        attackPowerMagic = 2;
        defenseMelee = 2;
        defenseRanged = 1;
        defenseMagic = 3;
        hitRateMelee = 0.7f;
        hitRateRanged = 0.0f;
        hitRateMagic = 0.5f;
        evasionMelee = 0.2f;
        evasionRanged = 0.1f;
        evasionMagic = 0.3f;
        attackSpeed = 1.2f;
        movementSpeed = 0.8f;
        experienceReward = 10;
        courage = 1;
        description = "A weak, gelatinous creature that barely holds its form together.";
    }
    public override void TakeDamage(int damage, string nameOfSpecies = "???")
    {
        base.TakeDamage(damage,nameOfSpecies);
    }
}
