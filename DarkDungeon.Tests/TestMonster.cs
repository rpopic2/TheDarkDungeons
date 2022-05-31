using System;

public class TestMonster : Monster, ISpawnable
{
    public static StatInfo TestMonStat = new(new(1, 1, 1), 3, 1);
    public static MonsterData TestMonData = new("TestMon", 't', 'T', TestMonStat, new Item[] { });
    public TestMonster(Position spawnPoint, MonsterData? data = null) : base(TestMonData, spawnPoint)
    {
        _currentMap.UpdateFightable(this);
    }

    public Monster Instantiate(Position spawnPoint) => new TestMonster(spawnPoint, TestMonData);

    protected override void OnEnergyDeplete()
    {
        throw new NotImplementedException();
    }

    protected override void OnTarget()
    {

    }
    protected override void OnNothing() { }

    public void GiveItem(Item item)
    {
        Inven.Add(item);
    }
    internal void SetHp(int v)
    {
        int increment = v - Stat.Hp.Cur;
        Stat.Hp.IncreaseMax(increment);
    }

    internal void SetStat(StatName statName, int v)
    {
        Stat[statName] = v;
    }

    internal void LevelUp()
    {
        Stat.Level++;
    }
}
