public class Monster : Moveable
{
    private int expOnKill;
    private static readonly Player player = Player.instance;
    public Monster(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con, int expOnKill, Position spawnPoint) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        this.expOnKill = expOnKill;
        for (int i = 0; i < cap; i++)
        {
            int attack = rnd.Next(0, 11);
            Card card = Draw();
            if(attack > 2) card.StanceShift();
            Pickup(card);
        }
        Pos = spawnPoint;
    }
    public void Pickup(Card card)
    {
        Hand.SetAt(Hand.Count, card);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        player.Loot(expOnKill, Draw());
        Map.Current.SpawnMob();
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
                _UseCard(Hand.GetFirst());
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
        Pickup(Draw());
    }
    public void UseCard()
    {
        Card? card = Hand.GetFirst();
    }
    public override char ToChar()
    {
        return Pos.facing == Facing.Front ? 'b' : 'd';
    }
}