using System;

[Flags]
public enum ItemFlag
{
    None = 1 << 0,
    Consumable = 1 << 1,
    Equip = 1 << 2,
    Currency = 1 << 3,
    Material = 1 << 4,
    QuestItem = 1 << 5,
    Unique = 1 << 6
}
