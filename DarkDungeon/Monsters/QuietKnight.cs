namespace Entities;
public class QuietKnight : Monster, ISpawnable
{
    private static StatInfo stat = new(new(4, 2, 2), 5, 10);
    private static MonsterData data = new("조용한 기사", 'ì', 'í', stat, new Item[] { mutedSword });
    private int[][] patterns = new int[][]{
        new int[] { 0, 0, 0 },
        new int[] { 1, 2 },
        new int[] { 1 }
    };
    private Queue<int> queue;

    public QuietKnight(Position spawnPoint) : base(data, spawnPoint)
    {
        Stat.AddSight(4);
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
    protected override void OnFollowTarget()
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

    public Monster Instantiate(Position spawnPoint) => new QuietKnight(spawnPoint);
}
public partial class Fightable
{
    public readonly static Item mutedSword = new("무음의 검", ItemType.Equip, new IBehaviour[]
     {
            new Skill("소리 없는 칼날", StanceName.Offence, StatName.Sol, DamageType.Slash, ".....", (i)=>i.Attack(2) ),
            new Charge("칼 들어올리기", StatName.Sol, DamageType.Magic, ".....!!!", (i)=>{i.Charge(mutedSword!);}), new Skill("내려치기", StanceName.Offence, StatName.Sol, DamageType.Normal, "!!!.....", (i)=>i.Attack(2) ),
new Skill("돌진", StanceName.Offence, StatName.Sol, DamageType.Normal, "!!!!!!", (i)=>{i.Dash(new Position(4, i.Pos.facing));i.Attack(4);i._lastHit?.Status.SetStun(1);})


     });

}
