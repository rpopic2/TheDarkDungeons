public class Inventoriable : Fightable
{
    public Inventory<ItemEntity?> Inven { get; private set; }

    public Inventoriable(string name, ClassName className, int cap, int maxHp, int level, int sol, int lun, int con) : base(name, className, cap, maxHp, level, sol, lun, con)
    {
        Inven = new Inventory<ItemEntity?>(3, "Inventory");
    }

    public void UseInven(int index)
    {
        if (!(Inven[index] is ItemEntity item)) return;
        if (!(item.onUse is Action<Inventoriable> onUse)) return;
        stance = new(Stance.Item, default);
        onUse(this);
        if (item.itemType == ItemType.Consum)
        {
            item.stack--;
            if (item.stack <= 0) Inven.Delete(index);
        }
    }
    protected virtual void Pickup(ItemEntity item, int index)
    {
        if (Inven[index] is ItemEntity oldEntity)
        {
            if (oldEntity.itemType == ItemType.Consum && oldEntity.abv == item.abv) oldEntity.stack++;
            else if (oldEntity.itemType == ItemType.Equip)
            {
                oldEntity.onExile?.Invoke(this);
                Inven[index] = null;
            }
        }
        if (Inven[index] is null)
        {
            Inven[index] = item;
            if (item.itemType == ItemType.Equip) item.onUse?.Invoke(this);
        }
    }
    public static class Data
    {
        public static readonly Item HpPot = new("HPPOT", ItemType.Consum, f => f.Hp += 3);
        public static readonly Item Torch = new("TORCH", ItemType.Consum, f =>
        {
            Player player = (Player)f;
            player.torch = 20;
            Program.OnTurnEnd -= torchHandler;
            Program.OnTurnEnd += torchHandler;
        });
        public static Action<object?, EventArgs> torchAct = (object? sender, EventArgs e) =>
            {
                Player player = Player.instance;
                if (player.torch > 0)
                {
                    player.sight = 3;
                    player.torch--;
                    if (player.torch <= 0)
                    {
                        player.sight = 1;
                    }
                }
            };
        public static EventHandler torchHandler = new EventHandler(torchAct);
        public static readonly Item Scouter = new("SCOUTR", ItemType.Skill, f => IO.pr(f.Target?.ToString() ?? "No Target to scout."));
        public static readonly Item AmuletOfLa = new("AMULLA", ItemType.Equip, f => f.Sol += 20, f => f.Sol -= 20);
        public static readonly Item FieryRing = new("FIRING", ItemType.Equip, f => f.Sol += 3, f => f.Sol -= 3);
        public static readonly Item Bag = new(" BAG  ", ItemType.Consum, f => f.Inven.Cap += 2);
        public static readonly Item Charge = new("CHARGE", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            f.Move(1);
            f.Move(1);
            f.UseCard(card);
        });
        public static readonly Item ShadowAttack = new("SHADOW", ItemType.Skill, f =>
        {
            if (f.SelectCard() is Card card)
            {
                Card newCard = new(card.Lun, card.Sol, card.Con, CardStance.Attack);
                f.UseCard(newCard);
            }
        });
        public static readonly Item SNIPE = new("SNIPE ", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            Map.Current.Moveables.TryGet(Player.instance.Pos.x + 2, out Moveable? target);
            Player.instance.Target = target;
            f.UseCard(card);
        });
        public static readonly Item Berserk = new("BERSRK", ItemType.Skill, f =>
         {
             Card? card = f.SelectCard();
             if (card?.Stance == CardStance.Attack) f.stance.amount += f.Hp.Max - f.Hp.Cur;
             f.UseCard(card);
         });
        public static readonly Item Backstep = new("BKSTEP", ItemType.Skill, f =>
          {
              if (f.Target is not null)
              {
                  if (f is Moveable mov)
                  {
                      Inventoriable target = (Inventoriable)f.Target;
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