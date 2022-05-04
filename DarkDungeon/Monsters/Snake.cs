namespace Entities;
public class Snake : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 3, 1), energy: 3, killExp: 5);
    public static MonsterData data = new(name: "뱀", 'S', '2', stat, new Item[] { Fightable.snakeItem, Fightable.poison });
    private bool _hissed = false;

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
            metaData["hissed"] = 1;
            return;
        }
        else if (_target!.Pos.Distance(Pos) <= 1)
        {
            _SelectSkill(0, 1); //bite
        }
        else
        {
            FollowTarget();
        }
    }
    protected override void OnNothing()
    {
        BasicMovement();
    }

    public Monster Instantiate(Position spawnPoint) => new Snake(spawnPoint);
}