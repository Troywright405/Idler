using System;

[Flags]
public enum ItemFlag : ulong //Bitwise system of flags, each 0 or 1 in the sequence represents one of these variables. 32 bit Int supports 31 flags (Uint would be 32)
{
    None = 1UL << 0,
    Consumable = 1UL << 1,
    Equip = 1UL << 2,
    Currency = 1UL << 3,
    Material = 1UL << 4,
    QuestItem = 1UL << 5,
    Unique = 1UL << 6,

    //This range is reserved for item rarities
    Worn = 1UL << 30,
    Flawed = 1UL << 31,
    Common = 1UL << 32,
    Uncommon = 1UL << 33,
    Rare = 1UL << 34,
    Epic = 1UL << 35,
    Legendary = 1UL << 36,
    Mythic = 1UL << 37,
    Divine = 1UL << 38,

    //This range reserved for item major traits, not sure if worth implementing?
    Cursed = 1UL << 40, // Includes major debuffs while equipped?
    Blessed = 1UL << 41, // Includes buffs/holy buffs dependant on earning?
    Ascended = 1UL << 42, // Maybe we can support a single tier of Ascended gear? Maybe upgrading it works completely differently
    Sealed = 1UL << 43, // Lot of potential, temporarily nerfed item, great as a multi-quest tier-ing reward? Maybe turns into Blessed or Ascended
    Unknown = 1UL << 44, // Strange gear/item that doesn't display what it does until its researched/discovered, maybe can turn into a Sealed item
    Explosive = 1UL << 45, // Damage to both target and player/blasts of energy with every impact
    Reaping = 1UL << 46 // Opposite of explosive, siphons hp
    
}
