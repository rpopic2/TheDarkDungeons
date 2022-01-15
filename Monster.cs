public class Monster : Entity
{
    private int expOnKill;
    private static readonly Player player = Player.instance;
    public Monster(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con, int expOnKill) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        this.expOnKill = expOnKill;
        for (int i = 0; i < cap; i++)
        {
            Pickup(Draw().StanceShift());
        }
    }
    public void Pickup(Card card)
    {
        Hand.SetAt(Hand.Count, card);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        player.exp.Gain(expOnKill);
        player.Pickup(Draw());
        player.target = null;
    }
    public void DoTurn()
    {
        if (IsAlive && target is not null)
            if (Hand.Count > 0)
            {
                _UseCard(Hand.GetFirst());
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
    public override string ToString()
    {
        if (IsAlive) return Name[0].ToString().ToLower();
        return MapSymb.empty;
    }
}