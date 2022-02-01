public class Monster : Fightable
{
    private int expOnKill;
    private static Player player { get => Player.instance; }
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
    }

    protected override void OnDeath(object? sender, EventArgs e)
    {
        base.OnDeath(sender, e);
        player.exp.Gain(expOnKill);
        player.PickupCard(Draw());
        Map.Current.SpawnBat();
        if (Drop(10)) player.PickupItem(Inventoriable.Data.HpPot);
        if (Drop(10)) player.PickupItem(Inventoriable.Data.FieryRing);
        if (Drop(5)) player.PickupItem(Torch.data);
        if (Drop(100)) player.PickupItem(Inventoriable.Data.AmuletOfLa);
        if (Drop(4)) player.PickupItem(Inventoriable.Data.Scouter);
        if (Drop(10)) player.PickupItem(Inventoriable.Data.Charge);
        if (Drop(20)) player.PickupItem(Inventoriable.Data.SNIPE);
        if (Drop(10)) player.PickupItem(Inventoriable.Data.ShadowAttack);
        if (Drop(12)) player.PickupItem(Inventoriable.Data.Berserk);
        if (Drop(20)) player.PickupItem(Inventoriable.Data.Backstep);
        if (Drop(11)) player.PickupItem(Inventoriable.Data.Bag);
    }
    private bool Drop(int outof)
    {
        int drop = rnd.Next(0, outof);
        return drop == 0;
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
                if (Map.Current.IsAtEnd(Pos.x)) _Move(direction, out char obj);
                else _Move(moveX, out char obj);
            }
            else
            {
                _UseCard((Card)Hand.GetFirst()!);
            }
        }
        else
        {
            Rest();
        }
    }
    public override void Rest()
    {
        base.Rest();
        PickupCard(Draw(), Hand.Count);
    }
    public override char ToChar()
    {
        return Pos.facing == Facing.Front ? 'b' : 'd';
    }
}

public readonly record struct MonsterInfo(string name, ClassName className, (int, float, float) hp, PointInfo cap, PointInfo sol, PointInfo lun, PointInfo con, PointInfo killExp);
public readonly record struct PointInfo(int basePoint, float pointPerLevel = default, float pointPerTurn = default);