public class Monster : Entity
{
    private static readonly Player player = Player.instance;
    public Monster(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        for (int i = 0; i < cap; i++)
        {
            Pickup(Draw().StanceShift());
        }
        target = player;//temp
    }
    public void Pickup(Card card)
    {
        Hand.SetAt(Hand.Count, card);
    }
    protected override void OnDeath()
    {
        base.OnDeath();
        player.exp.Gain(3);
        player.Pickup(Draw());
    }
    public void DoTurn()
    {
        if (Hand.Count > 0)
        {
            UseCard(Hand.GetFirst());
        }else
        {
            IO.pr($"{Name} is resting a turn.");
            Pickup(Draw());
        }
    }

    public void UseCard()
    {
        Card? card = Hand.GetFirst();
    }
}