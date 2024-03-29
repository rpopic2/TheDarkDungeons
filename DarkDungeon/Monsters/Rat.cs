public class Rat : Monster, ISpawnable
{
    private static StatInfo stat = new(new(1, 2, 1), 4, 3);
    public static readonly MonsterData data = new("거대 쥐", 'R', 'Я', stat, new ItemOld[] { ratItem });
    public Rat(Position spawnPoint) : base(data, spawnPoint)
    {
        Stat.AddSight(3);
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnTarget()
    {
        int distance = DistanceToTarget;
        if (distance == 2) _SelectSkill(0, 1);
        else if (distance == 1) _SelectSkill(0, 0);
        else
        {
            FollowTarget();
        }
    }

    public Monster Instantiate(Position spawnPoint) => new Rat(spawnPoint);

    protected override void OnNothing()
    {
        BasicMovement();
    }
}
