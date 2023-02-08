public class QuietKnight : Monster, ISpawnable
{
    private static StatInfo stat = new(new(sol: 3, lun: 2, con: 2), energy: 4, killExp: 50);
    public static readonly MonsterData data = new("조용한 기사", 'ì', 'í', stat, new ItemOld[] { mutedSword });
    private int[][] patterns = new int[][]{
        new int[] { 0, 0, 0 },
        new int[] { 1, 2 },
        new int[] { 1 }
    };
    private Queue<int> queue;

    public QuietKnight(Position spawnPoint) : base(data, spawnPoint)
    {
        Stat.AddSight(3);
        queue = new(AddPattern(0, 1));
    }
    private int[] AddPattern(int x, int y)
    {
        var result = patterns[x].Append(-1);
        result = result.Concat(patterns[y]);
        return result.ToArray();
    }
    protected override void OnNothing() => BasicMovement();
    protected override void OnEnergyDeplete()
    {
        SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    protected override void OnTarget()
    {
        int distance = DistanceToTarget;
        if (distance > 2)
        {
            _SelectSkill(0, 3);
            return;
        }
        if (queue.Count <= 0)
        {
            _SelectSkill(0, 0);
        }
        int action = queue.Dequeue();
        if (action == -1) OnEnergyDeplete();
        else _SelectSkill(0, action);
    }
    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        Map.Current.SpawnPortal(Pos);
    }

    public Monster Instantiate(Position spawnPoint) => new QuietKnight(spawnPoint);
}
public partial class Creature
{
    public readonly static ItemOld mutedSword = new("무음의 검", ItemType.Equip, new IBehaviour[]
     {
            new SkillOld("소리 없는 칼날", StanceName.Offence, StatName.Sol, DamageType.Slash, ".....", (i)=>i.Attack(2) ),
            new Charge("칼 들어올리기", StatName.Sol, DamageType.Magic, ".....!!!(조용한 기사가 칼을 힘껏 들어올렸다!)", (i)=>{i.Charge(mutedSword!);}), new SkillOld("내려치기", StanceName.Offence, StatName.Sol, DamageType.Normal, "!!!.....(조용한 기사는 칼을 휘둘렀고, 검은 소리 없이 허공을 갈랐다.)", (i)=>i.Attack(2) ),
new SkillOld("돌진", StanceName.Offence, StatName.Sol, DamageType.Normal, "!!!!!!(조용한 기사가 소리 없이 달려온다!)", (i)=>{i.Dash(new Position(3, i.Pos.facing));i.LastHit?.CurAction.SetStun(1);})
     });

}
