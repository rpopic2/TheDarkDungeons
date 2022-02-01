public class Monster : Fightable
{
    private int expOnKill;
    private static Player player { get => Player.instance; }
    private (IItemData data, int outof)[] dropList;
    public Monster(string name, ClassName className, int lv, int maxHp, int cap, (int sol, int lun, int con) stat, int expOnKill, Position spawnPoint) : base(name, className, cap, maxHp, lv, stat.sol, stat.lun, stat.con)
    {
        this.expOnKill = expOnKill;
        for (int i = 0; i < cap; i++)
        {
            int attack = rnd.Next(0, 11);
            Card card = Draw();
            if (attack > 2) card.StanceShift();
            card.StanceShift();
            PickupCard(card, Hand.Count);
        }
        Pos = spawnPoint;
        dropList = new (IItemData data, int outof)[5]{
            (Inventoriable.ConsumeDb.HpPot, 10),
            (Inventoriable.ConsumeDb.Bag, 11),
            (Torch.torch, 5),
            (EquipDb.FieryRing, 15),
            (EquipDb.LunarRing, 15)
        };
    }

    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += expOnKill;
        player.PickupCard(Draw());
        Map.Current.SpawnBat();
        foreach (var item in dropList)
        {
            if(DropOutOf(item.outof)) player.PickupItem(item.data);
        }
    }
    public void DoTurn()
    {
        if (!IsAlive) return;

        if (Hand.Count > 0)
        {
            if (Target is null)
            {
                int moveX = rnd.Next(2) == 1 ? 1 : -1;
                int direction = Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(Pos.x)) Move(direction, out char obj);
                else Move(moveX, out char obj);
            }
            else _UseCard((Card)Hand.GetFirst()!);
        }
        else Rest();
    }
    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw(), Hand.Count);
    }
    private bool DropOutOf(int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? 'b' : 'd';
}

public readonly record struct MonsterInfo(string name, ClassName className, (int, float, float) hp, PointInfo cap, PointInfo sol, PointInfo lun, PointInfo con, PointInfo killExp);
public readonly record struct PointInfo(int basePoint, float pointPerLevel = default, float pointPerTurn = default);