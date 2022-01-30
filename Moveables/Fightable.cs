public class Fightable : Entity
{
    public ClassName ClassName { get; private set; }
    public Inventory<Card?> Hand { get; private set; }
    public Inventory<Item?> Inven { get; private set; }
    public GamePoint Hp { get; set; }
    public virtual Fightable? Target { get; protected set; }
    protected (Stance stance, int amount) stance = (default, default);
    public (Stance stance, int amount) CurStance => stance;
    private int star;
    public bool IsResting => stance.stance == Stance.Rest;
    public bool IsAlive => !Hp.IsMin;
    public Fightable(string name, ClassName className, int cap, int maxHp, int level, int sol, int lun, int con) : base(level, sol, lun, con, name)
    {
        ClassName = className;
        Hand = new Inventory<Card?>(cap, "Hand");
        Inven = new Inventory<Item?>(3, "Inventory");
        Hp = new GamePoint(maxHp, GamePointOption.Reserving);
        Hp.OnOverflow += new EventHandler(OnDeath);
        Hp.OnHeal += new EventHandler<PointArgs>(OnHeal);
        Hp.OnDamage += new EventHandler<PointArgs>(OnDamaged);
        Program.OnTurnEnd += new EventHandler(OnTurnEnd);
    }
    public virtual Card? PickCard() => Hand.GetFirst();

    public void UseCard(int index)
    {
        if (Target is null) return;
        if (Hand[index] is Card card)
        {
            if (card.Stance == CardStance.Star) IO.pr("Next move will be reinforced.");
            _UseCard(card);
        }
    }
    public void UseCard(Card? card)
    {
        if (Target is null) return;
        if (card is Card card2)
        {
            if (card2.Stance == CardStance.Star) IO.pr("Next move will be reinforced.");
            _UseCard(card2);
        }
    }
    protected void _UseCard(Card card)
    {
        Hand.Delete(card);
        switch (card.Stance)
        {
            case CardStance.Attack:
                stance.stance = Stance.Attack;
                stance.amount += card.Sol;
                break;
            case CardStance.Dodge:
                stance.stance = Stance.Dodge;
                stance.amount += card.Lun;
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
        //else if (IsAlive) IO.pr($"{Name} takes {damage} damage. {Hp}", true);
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
                if (item.itemType == ItemType.Consum) Inven.Delete(index);
            }
        }
    }
    public virtual void Rest()
    {
        if (this is Player)
            IO.pr($"{Name} is resting a turn.");
        stance = (Stance.Rest, default);
    }

    public virtual void OnTurnEnd(object? sender, EventArgs e) => stance = (default, default);
    protected virtual void OnDeath(object? sender, EventArgs e) => IO.pr($"{Name} died.", false, true);
    protected void OnHeal(object? sender, PointArgs e) => IO.pr($"{Name} restored {e.Amount} hp. {Hp}", true);
    protected void OnDamaged(object? sender, PointArgs e)
    {
        if (e.Amount > 0) IO.pr($"{Name} takes {e.Amount} damage. {Hp}", true);
    }

    public override string ToString() =>
        $"Name : {Name}\tClass : {ClassName.ToString()}\tLevel : {Level}\nHp : {Hp}\tCap : {Hand.Cap}\tSol : {Sol}\tLun : {Lun}\tCon : {Con}";

    public virtual char ToChar()
    {
        if (IsAlive) return Name.ToLower()[0];
        else return MapSymb.Empty;
    }

    protected virtual void Pickup(Item item, int index)
    {
        if (Inven[index] is Item oldItem && oldItem.itemType == ItemType.Equip)
            oldItem.onExile?.Invoke(this);
        Inven[index] = item;
        if (item.itemType == ItemType.Equip) item.onUse?.Invoke(this);
    }

    public virtual void Pickup(Card card, int index)
    {
        Hand[index] = card;
    }


    public static class ItemData
    {
        public static readonly Item HpPot = new("HPPOT", ItemType.Consum, f => f.Hp += 3);
        public static readonly Item Torch = new("TORCH", ItemType.Consum, f =>
        {
            Player player = (Player)f;
            player.torch = 20;
            Action<object?, EventArgs> act = (object? sender, EventArgs e) =>
            {
                if (player.torch > 0)
                {
                    player.sight = 3;
                    player.torch--;
                    if (player.torch <= 0)
                    {
                        player.Inven.Delete(Fightable.ItemData.Torch);
                        player.sight = 1;
                    }
                }
            };
            Program.OnTurnEnd += new EventHandler(act);

        });
        public static readonly Item Scouter = new("SCOUTR", ItemType.Skill, f => IO.pr(f.Target?.ToString() ?? "No Target to scout."));
        public static readonly Item AmuletOfLa = new("AMULLA", ItemType.Equip, f => f.Sol += 20, f => f.Sol -= 20);
        public static readonly Item FieryRing = new("FIRING", ItemType.Equip, f => f.Sol += 3, f => f.Sol -= 3);
        public static readonly Item Bag = new(" BAG  ", ItemType.Consum, f => f.Inven.Cap += 2);
        public static readonly Item Charge = new("CHARGE", ItemType.Skill, f =>
        {
            Card? card = f.PickCard();
            Moveable mov = (Moveable)f;
            mov.Move(1);
            mov.Move(1);
            f.UseCard(card);
        });
        public static readonly Item ShadowAttack = new("SHADOW", ItemType.Skill, f =>
        {
            if (f.PickCard() is Card card)
            {
                Card newCard = new(card.Lun, card.Sol, card.Con, CardStance.Attack);
                f.UseCard(newCard);
            }
        });
        public static readonly Item SNIPE = new("SNIPE ", ItemType.Skill, f =>
        {
            Card? card = f.PickCard();
            Map.Current.Moveables.TryGet(Player.instance.Pos.x + 2, out Moveable? target);
            Player.instance.Target = target;
            f.UseCard(card);
        });
        public static readonly Item Berserk = new("BERSRK", ItemType.Skill, f =>
         {
             Card? card = f.PickCard();
             if (card?.Stance == CardStance.Attack) f.stance.amount += f.Hp.Max - f.Hp.Cur;
             f.UseCard(card);
         });
        public static readonly Item Backstep = new("BKSTEP", ItemType.Skill, f =>
          {
              if (f.Target is not null)
              {
                  if (f is Moveable mov)
                  {
                      Fightable target = f.Target;
                      target.Target = null;
                      Map.Current.Tiles.TryGet(mov.Pos.FrontIndex + 1, out char obj);
                      if (obj == MapSymb.portal) return;
                      mov.Move(2 * mov.Pos.FrontMul);
                      mov.Move(1 * mov.Pos.BackMul);
                  }
              }
          });
    }
}