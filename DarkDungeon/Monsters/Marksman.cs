public class Marksman : Monster, ISpawnable
{
    public static readonly MonsterData data = new("그림자 궁사", ']', '[', new(new(sol: 0, lun: 2, con: 1), energy: 3, killExp: 5), new ItemOld[] { Creature.bow, Creature.torch });
    public Marksman(Position spawnPoint) : base(data, spawnPoint)
    {
        Stat.AddSight(-1); //횃불로 올라가서 다시 줄여줌
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
        BasicMovement();
        //가만히 있는걸로 바꾸기?
    }

    protected override void OnTarget()
    {
        if (Inven.Contains(Creature.arrow)) _SelectSkill(0, 0);//bow shoot
        else FollowTarget(true);
    }
}
