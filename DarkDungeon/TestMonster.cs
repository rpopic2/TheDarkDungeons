public class TestMonster : Monster, ISpawnable
{
    public static StatInfo TestMonStat = new(new(1, 1, 1), 3, 1);
    public static MonsterData TestMonData = new("TestMon", 't', 'T', TestMonStat, new Item[] {});
    public TestMonster(Position spawnPoint, MonsterData? data = null) : base(TestMonData, spawnPoint)
    {

    }

    public Monster Instantiate(Position spawnPoint) => new TestMonster(spawnPoint, TestMonData);

    protected override void OnEnergyDeplete()
    {
        throw new NotImplementedException();
    }

    protected override void OnTarget()
    {
        throw new NotImplementedException();
    }
    protected override void OnNothing()
    {

    }

    public void GiveItem(Item item)
    {
        Inven.Add(item);
    }

    public void SetAction(Item item, int skillIndex)
    {
        CurAction.Set(item, item.skills[skillIndex]);
    }
}
