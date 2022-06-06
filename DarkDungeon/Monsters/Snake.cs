public class Snake : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 3, 1), energy: 3, killExp: 5);
    public static readonly MonsterData data = new(name: "뱀", 'S', '2', stat, new ItemOld[] { Creature.snakeItem, Creature.poison });
    private bool _hissed = false;
    protected int Range = 1;

    public Snake(Position spawnPoint) : base(data, spawnPoint) { }

    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnTarget()
    {
        if (!_hissed)
        {
            _SelectSkill(0, 0); //hiss
            _hissed = true;
            return;
        }
        else if (DistanceToTarget > Range)
        {
            FollowTarget();
        }
        else
        {
            _SelectSkill(0, 1); //bite
        }
    }

    public Monster Instantiate(Position spawnPoint) => new Snake(spawnPoint);

    protected override void OnNothing()
    {
        BasicMovement();
    }
}