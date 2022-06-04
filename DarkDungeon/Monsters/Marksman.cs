public class Marksman : Monster, ISpawnable
{
    public static readonly MonsterData data = new("그림자 궁사", ']', '[', new(new(sol: 1, lun: 3, con: 1), energy: 3, killExp: 5), new Item[] { Creature.bow });
    public Marksman(Position spawnPoint) : base(data, spawnPoint)
    {
        ItemMetaData metaData = new(5);
        Inven.Add(Creature.arrow, metaData);
    }

    public Monster Instantiate(Position spawnPoint) => new Marksman(spawnPoint);

    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }

    protected override void OnNothing()
    {
        //은신 해야됨
    }

    protected override void OnTarget()
    {
        if (Inven.Contains(Creature.arrow)) _SelectSkill(0, 0);//bow shoot
        else FollowTarget(true);
    }
}
