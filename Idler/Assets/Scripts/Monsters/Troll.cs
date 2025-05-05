using System;

public class Troll : Monster
{
    public Troll()
    {
        nameOfSpecies = "Troll";
        maxHealth = 450;
        attackPowerMelee = 70;
        attackPowerRanged = 20;
        attackPowerMagic = 10;
        defenseMelee = 40;
        defenseRanged = 15;
        defenseMagic = 10;
        hitRateMelee = 0.70f;
        hitRateRanged = 0.50f;
        hitRateMagic = 0.30f;
        evasionMelee = 0.40f;
        evasionRanged = 0.20f;
        evasionMagic = 0.10f;
        attackSpeed = 1.3f;
        movementSpeed = 1.2f;
        experienceReward = 120;
        courage = 50;
        description = "A hulking brute with immense strength and regenerative abilities. Known for its ferocity in battle.";
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
