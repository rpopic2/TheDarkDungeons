public class Rat : Monster, ISpawnable
{
    private static StatInfo stat = new(new(1, 2, 1), 4, 3);
    private static MonsterData data = new("거대 쥐", 'R', 'Я', stat, new Item[] { ratItem });
    public Rat(Position spawnPoint) : base(data, spawnPoint)
    {
        Stat.AddSight(3);
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnFollowTarget()
    {
        int distance = DistanceToTarget;
        if (distance == 2) _SelectSkill(0, 1);
        else if (distance == 1) _SelectSkill(0, 0);
        else
        {
            Facing newFacing = Pos.LookAt(_followTarget!.Pos.x);
            SelectBasicBehaviour(0, 1, (int)newFacing); //mov
        }
    }
    protected override void OnNothing()
    {
        BasicMovement();
    }

    public Monster Instantiate(Position spawnPoint) => new Rat(spawnPoint);
}
