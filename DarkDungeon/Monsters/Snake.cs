namespace Entities;
public class Snake : Monster, ISpawnable
{
    private static StatInfo stat = new(stat: new(1, 3, 1), energy: 3, killExp: 5, Sight: 3);
    public static MonsterData data = new(name: "뱀", 'S', '2', stat, new Item[] { Fightable.snakeItem, Fightable.poison });

    public Snake(Position spawnPoint) : base(data, spawnPoint) { }

    public void SnakeBehav()
    {
        if (Energy.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (!metaData.ContainsKey("hissed"))
                {
                    _SelectSkill(0, 0); //hiss
                    metaData["hissed"] = 1;
                    return;
                }
                else if (_followTarget.Pos.Distance(Pos) <= 1 && Inven.GetMeta(Fightable.snakeItem).poison > 0)
                {
                    _SelectSkill(0, 1); //bite
                }
                else
                {
                    Facing newFacing = Pos.LookAt(_followTarget.Pos.x);
                    SelectBasicBehaviour(0, 1, (int)newFacing); //move
                }
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnFollowTarget()
    {
        if (!metaData.ContainsKey("hissed"))
        {
            _SelectSkill(0, 0); //hiss
            metaData["hissed"] = 1;
            return;
        }
        else if (_followTarget!.Pos.Distance(Pos) <= 1 && Inven.GetMeta(Fightable.snakeItem).poison > 0)
        {
            _SelectSkill(0, 1); //bite
        }
        else
        {
            Facing newFacing = Pos.LookAt(_followTarget.Pos.x);
            SelectBasicBehaviour(0, 1, (int)newFacing); //move
        }
    }
    protected override void OnNothing()
    {
        BasicMovement();
    }

    public Monster Instantiate(Position spawnPoint) => new Snake(spawnPoint);
}