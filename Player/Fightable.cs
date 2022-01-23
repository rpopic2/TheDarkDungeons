public class Fightable : Entity
{
    public ClassName ClassName { get; private set; }
    public Hand Hand { get; private set; }
    public Inventory Inven { get; private set; }
    public GamePoint Hp { get; set; }
    public virtual Fightable? Target { get; protected set; }
    protected (Stance stance, int amount) stance = (default, default);
    public (Stance stance, int amount) CurStance => stance;
    private int star;
    public bool IsResting => stance.stance == Stance.Rest;
    public bool IsAlive => !Hp.IsMin;
    public bool DidPrint => CurStance.stance != Stance.Move && CurStance.stance != Stance.None;

    public Fightable(string name, ClassName className, int cap, int maxHp, int level, int sol, int lun, int con) : base(level, sol, lun, con, name)
    {
        ClassName = className;
        Hand = new Hand(cap);
        Inven = new Inventory(3);
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
    }
    public virtual void UseCard(int index)
    {
        if (Target is null) return;
        if (Hand[index] is Card card)
        {
            if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced.");
            _UseCard(card);
        }
    }
    protected void _UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case CardStance.Attack:
                stance = (Stance.Attack, card.Sol);
                break;
            case CardStance.Dodge:
                stance = (Stance.Dodge, card.Lun);
                break;
            case CardStance.Star:
                star = card.Con;
                break;
        }
    }
    public void TryAttack()
    {
        if (!IsAlive || Target is null) return;
        if (stance.stance == Stance.Attack)
        {
            string tempString = $"{Name} attacks with {stance.amount} damage.";
            TryUseStar();
            IO.pr(tempString);
            Target.TryDodge(stance.amount);
        }
        else if (Target.stance.stance == Stance.Dodge) Target.TryDodge(0);
    }
    private void TryDodge(int damage)
    {
        if (stance.stance == Stance.Dodge)
        {
            string tempStr = $"{Name} dodges {stance.amount} damage.";
            TryUseStar();
            if (damage <= 0)
            {
                IO.pr(tempStr += "..but oppenent did not attack...");
                return;
            }
            IO.pr(tempStr);
            damage -= stance.amount;
        }
        else if (damage > 0 && IsResting)
        {
            damage = (int)MathF.Round(damage * Rules.vulMulp);
            IO.pr($"{Name} is resting vulnerable, takes {Rules.vulMulp}x damage! (total {damage})");
        }
        Hp -= damage;
        if (damage <= 0) IO.pr($"{Name} completely dodges. {Hp}", true);
        else if (IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp}", true);
    }
    private void TryUseStar()
    {
        if (star <= 0) return;
        stance.amount += star;
        IO.pr($"..and {star} more damage! (total {stance.amount})");
        star = 0;
    }
    public void UseInven(int index)
    {
        if (Inven[index] is Item item)
        {
            if (item.onUse is Action<Fightable> onUse)
            {
                stance = (Stance.Item, default);
                onUse(this);
                if (item.isConsumeable) Inven[index] = null;
            }
        }
    }
    public virtual void Rest()
    {
        if (this is not Player && Map.Current.IsVisible((Moveable)this))
            IO.pr($"{Name} is resting a turn.");
        stance = (Stance.Rest, default);
    }

    public virtual void OnTurnEnd()
    {
        stance = (default, default);
    }
    protected virtual void OnDeath(object? sender, EventArgs e) => IO.pr($"{Name} died. {Hp}", true, true);
    protected void OnHeal(object? sender, HealArgs e) => IO.pr($"{Name} restored {e.Amount} hp. {Hp}", true);

    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\tCap : {Hand.Cap}\tSol : {Sol}\tLun : {Lun}\tCon : {Con}";

    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        else return MapSymb.invisible;
    }
    public static class ItemData
    {
        public static readonly Item HpPot = new("HPPOT", null, f => f.Hp += 3, true);
        public static readonly Item AmuletOfLa = new("AMULA", f => f.Sol += 20, null);
        public static readonly Item Dash = new("DASH", null, f => ((Moveable)f).Move(2));
    }
}
public readonly record struct Item(string abv, Action<Fightable>? onPickup, Action<Fightable>? onUse, bool isConsumeable = false)
{
    public override string ToString()
    {
        return $"[{abv}]";
    }
}