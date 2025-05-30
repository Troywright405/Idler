using System.Collections.Generic;

public static class AreaDatabase
{
    public static Dictionary<int, Area> Areas = new();

    static AreaDatabase()
    {
        Areas[1] = new Area(1, "Slime nest", SpawnListDatabase.Get("firstSpawns"));
        Areas[2] = new Area(2, "Goblin breeding site", SpawnListDatabase.Get("strongerSpawns"));
    }

    public static Area Get(int areaNumber)
    {
        return Areas.TryGetValue(areaNumber, out var area) ? area : null;
    }
    
}
public class Area
{
    public int AreaNumber;      // index
    public string Name;
    public SpawnList Spawns;

    public Area(int number, string name, SpawnList spawns)
    {
        AreaNumber = number;
        Name = name;
        Spawns = spawns;
    }
}
