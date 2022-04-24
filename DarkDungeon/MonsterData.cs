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
    private static StatMul lunaticMul = new(sol: 1, lun: 1, con: 3, cap: 4, killExp: 3, Sight: 1);
    public static MonsterData lunatic = new(name: "광신도", '>', '<', lunaticMul, (m) => m.LunaticBehav(), new Item[] { Fightable.holySword, Fightable.tearOfLun }, startToken: new int[] { 2, 0, 2, 0 });
    public static StatMul batMul = new(sol: 1, lun: 3, con: 2, cap: 3, killExp: 4, Sight: 1);
    public static MonsterData bat = new(name: "박쥐", 'b', 'd', batMul, (m) => m.BatBehav(), new Item[] { Fightable.batItem }, startToken: new int[] { 1, 1, 0 });
    private static StatMul snakeMul = new(sol: 1, lun: 3, con: 1, cap: 3, killExp: 5, Sight: 3);
    public static MonsterData snake = new(name: "뱀", 'S', '2', snakeMul, (m) => m.SnakeBehav(), new Item[] { Fightable.snakeItem }, startToken: new int[] { 2, 0, 0 });
    private static StatMul shamanMul = new(sol: 0, lun: 1, con: 1, cap: 4, killExp: 4, Sight: 4);
    public static MonsterData shaman = new(name: "정령술사", '}', '{', shamanMul, (m) => m.ShamanBehav(), new Item[] { Fightable.spiritStaff, Fightable.torch }, startToken: new int[] { 2, 0, 2 });

    public static List<MonsterData> data = new() { bat, shaman, lunatic, snake };
    private Fightable? _followTarget;
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
        if (target is not Fightable || !this.IsEnemy(target)) this._followTarget = null;
        else this._followTarget = target;
    }
    public void BatBehav()
    {
        if (Tokens.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (metaData["isAngry"] == 1)
                {
                    _SelectSkill(0, 0);
                    metaData["isAngry"] = 0;
                }  //들이박기
                else if (Tokens.Cur <= 1) _SelectSkill(0, 0); //들이박기
                else _SelectSkill(0, 1); //구르기
            }
        }
        else
        {
            SelectBasicBehaviour(1, 0, -1); //pickup offence
        }
        if (_followTarget?.Stance.CurrentBehav?.Stance == StanceName.Charge) metaData["isAngry"] = 1;
        else metaData["isAngry"] = 0;
    }
    public void LunaticBehav()
    {
        if (GetHp().Cur != GetHp().Max && Inven.Content.Contains(Fightable.tearOfLun)) _SelectSkill(1, 0);
        else if (Tokens.Cur > 0)
        {
            if (_followTarget is null) BasicMovement();
            else
            {
                if (Inven.GetMeta(Fightable.holySword).magicCharge > 0) _SelectSkill(0, 0);
                else _SelectSkill(0, 1);
            }
        }
        else SelectBasicBehaviour(1, 0, -1); //pickup offence
    }
    public void SnakeBehav()
    {
        if (Tokens.Cur > 0)
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
                else if (_followTarget.Pos.Distance(Pos) <= 1 && Inven.GetMeta(Fightable.snakeItem).isPoisoned)
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
    private void ShamanBehav()
    {
        if (Tokens.Cur <= 0)
        {
            SelectBasicBehaviour(1, 0, -1); //pickup offence
            return;
        }
        else if (_followTarget is null)
        {
            BasicMovement();
            return;
        }
        else if (Inven.GetMeta(Fightable.spiritStaff).magicCharge > 0) _SelectSkill(0, 0);
        else _SelectSkill(0, 1);
    }
}
