public class Monster : Entity
{
    private static readonly Player player = Player.instance;
    public Monster(string name, ClassName className, int cap, int maxHp, int lv, int sol, int lun, int con) : base(name, className, cap, maxHp, lv, sol, lun, con)
    {
        for (int i = 0; i < cap; i++)
        {
            Pickup(Draw());
        }
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

    public void UseCard()
    {
        UseCard(Hand.GetFirst());
    }
}