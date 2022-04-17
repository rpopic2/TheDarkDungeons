namespace Entities;
public record MonsterData(string name, char fowardChar, char backwardChar, StatMul stat, Action<Monster> behaviour, Item[] startItem, int[] startToken);
public partial class Monster
{
    private static DropList lunDropList = new(
        (Fightable.holySword, 10));
    private static DropList snakeDropList = new(
        (Fightable.holySword, 10));
    private static DropList batDropList = new(
        (Fightable.batItem, 10));
    private static StatMul lunaticMul = new(sol: 1, lun: 1, con: 3, cap: 4, killExp: 3);
    public static MonsterData lunatic = new(name: "광신도", '>', '<', lunaticMul, (m) => m.LunaticBehav(), new Item[] { Fightable.holySword, Fightable.tearOfLun }, startToken: new int[] { 2, 0, 2, 0 });
    public static StatMul batMul = new(sol: 1, lun: 3, con: 2, cap: 3, killExp: 4);
    public static MonsterData bat = new(name: "박쥐", 'b', 'd', batMul, (m) => m.BatBehav(), new Item[] { Fightable.batItem }, startToken: new int[] { 1, 1, 0 });
    private static StatMul snakeMul = new(sol: 2, lun: 2, con: 1, cap: 3, killExp: 3);
    public static MonsterData snake = new(name: "뱀", 'S', '2', snakeMul, (m) => m.SnakeBehav(), new Item[] { Fightable.snakeItem }, startToken: new int[] { 2, 0, 0 });

    public static List<MonsterData> data = new() { lunatic, bat };
    private Fightable? _target;
    public static int Count => data.Count;
    private void BasicMovement()
    {
        int randomFace = Stat.rnd.Next(2);
        if (!Map.Current.IsAtEnd(Pos.x)) SelectBasicBehaviour(0, 1, randomFace);
        else SelectBasicBehaviour(0, 1, 1);//Move(new(1, Facing.Left));
    }
    private void _SelectSkill(int item, int skill)
    {
        SelectBehaviour(Inven[item]!, skill);
    }
    public override void OnTurnEnd()
    {
        UpdateTarget();
        base.OnTurnEnd();
    }
    private void UpdateTarget()
    {
        Fightable? target = _currentMap.RayCast(Pos, Sight);
        if (target is not Fightable || !this.IsEnemy(target)) this._target = null;
        else this._target = target;
    }
    public void BatBehav()
    {
        if (tokens.Count > 0)
        {
            if (_target is null) BasicMovement();
            else
            {
                if (metaData["isAngry"] == 1 && tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
                else if (tokens.Contains(TokenType.Defence)) _SelectSkill(0, 1);
                else if (tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
            }
        }
        else
        {
            SelectBasicBehaviour(1, 0, -1); //pickup offence
        }
        if (_target?.Stance.CurrentBehav?.Stance == StanceName.Charge) metaData["isAngry"] = 1;
        else metaData["isAngry"] = 0;
    }
    public void LunaticBehav()
    {
        if (GetHp().Cur != GetHp().Max && Inven.Content.Contains(Fightable.tearOfLun)) _SelectSkill(1, 0);
        else if (tokens.Count > 0)
        {
            if (_target is null) BasicMovement();
            else
            {
                if (Inven.GetMeta(Fightable.holySword).magicCharge > 0 && tokens.Contains(TokenType.Offence)) _SelectSkill(0, 0);
                else if (tokens.Contains(TokenType.Charge)) _SelectSkill(0, 1);
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    public void SnakeBehav()
    {
        if (tokens.Count > 0)
        {
            if (_target is null) BasicMovement();
            else
            {
                if (Inven.GetMeta(Fightable.snakeItem).magicCharge > 0 && tokens.Contains(TokenType.Offence)) _SelectSkill(0, 1);
                else if (tokens.Contains(TokenType.Charge)) _SelectSkill(0, 0);
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
}
