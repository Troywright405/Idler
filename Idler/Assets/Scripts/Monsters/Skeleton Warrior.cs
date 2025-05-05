using System;

public class SkeletonWarrior : Monster
{
    public SkeletonWarrior()
    {
        nameOfSpecies = "Skeleton Warrior";
        maxHealth = 180;
        attackPowerMelee = 40;
        attackPowerRanged = 10;
        attackPowerMagic = 5;
        defenseMelee = 30;
        defenseRanged = 20;
        defenseMagic = 10;
        hitRateMelee = 0.80f;
        hitRateRanged = 0.50f;
        hitRateMagic = 0.40f;
        evasionMelee = 0.20f;
        evasionRanged = 0.10f;
        evasionMagic = 0.30f;
        attackSpeed = 1.1f;
        movementSpeed = 1.0f;
        experienceReward = 70;
        courage = 40;
        description = "Reanimated bones of the fallen. These warriors lack magic but excel in physical combat.";
    }

    public override void TakeDamage(int damage, string nameOfSpecies)
    {
        base.TakeDamage(damage,nameOfSpecies);
    }
    public override void InitializeDropTable()
    {
        dropTable = new MonsterDropTable();
        dropTable.AddDrop("Gold", 1.0f);
    }
}
