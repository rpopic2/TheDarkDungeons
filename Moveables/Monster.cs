public class Monster : Fightable
{
    private int killExp;
    private static Player player { get => Player.instance; }
    private (IItemData data, int outof)[] dropList;
    private char fowardChar, backwardChar;
    private Action<Monster> behaviour;
    // public Monster(string name, ClassName className, int lv, int maxHp, int cap, (int sol, int lun, int con) stat, int expOnKill, Position spawnPoint) : base(name, className, cap, maxHp, lv, stat.sol, stat.lun, stat.con)
    // {
    //     this.expOnKill = expOnKill;
    //     for (int i = 0; i < cap; i++)
    //     {
    //         int attack = rnd.Next(0, 11);
    //         Card card = Draw();
    //         if (attack > 2) card.StanceShift();
    //         card.StanceShift();
    //         PickupCard(card, Hand.Count);
    //     }
    //     Pos = spawnPoint;
    //     dropList = new (IItemData data, int outof)[5]{
    //         (Inventoriable.ConsumeDb.HpPot, 10),
    //         (Inventoriable.ConsumeDb.Bag, 11),
    //         (Torch.torch, 5),
    //         (EquipDb.FieryRing, 15),
    //         (EquipDb.LunarRing, 15)
    //     };
    // }
    public Monster(MonsterData data, Position spawnPoint) : base(data.name, data.className, Map.level, data.stat.sol, data.stat.lun, data.stat.con, data.stat.hp, data.stat.cap)
    {
        dropList = data.dropList;
        killExp = data.stat.killExp;
        fowardChar = data.fowardChar;
        backwardChar = data.backwardChar;
        behaviour = data.behaviour;
        Pos = spawnPoint;
    }

    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.point += killExp;
        player.PickupCard(Draw());
        Map.Current.SpawnBat();
        foreach (var item in dropList)
        {
            if (DropOutOf(item.outof)) player.PickupItem(item.data);
        }
    }
    public void DoTurn()
    {
        if (!IsAlive) return;
        behaviour(this);
    }
    public static Action<Monster> batBehav = (m) =>
    {
        if (m.Hand.Count > 0)
        {
            if (m.Target is null)
            {
                int moveX = m.rnd.Next(2) == 1 ? 1 : -1;
                int direction = m.Pos.facing == Facing.Front ? -1 : 1;
                if (Map.Current.IsAtEnd(m.Pos.x)) m.Move(direction, out char obj);
                else m.Move(moveX, out char obj);
            }
            else m._UseCard((Card)m.Hand.GetFirst()!);
        }
        else m.Rest();
    };
    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw(), Hand.Count);
    }
    private bool DropOutOf(int outof) => rnd.Next(0, outof) == 0;
    public override char ToChar() => Pos.facing == Facing.Front ? 'b' : 'd';
}