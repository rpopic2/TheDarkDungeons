public class Inventoriable : Fightable
{
    public Inventory<IItemEntity?> Inven { get; private set; }

    public Inventoriable(string name, ClassName className, int cap, int maxHp, int level, int sol, int lun, int con) : base(name, className, cap, maxHp, level, sol, lun, con)
    {
        Inven = new Inventory<IItemEntity?>(3, "Inventory");
    }

    public void UseInven(int index)
    {
        if (!(Inven[index] is IItemEntity item)) return;
        if (!(item.onUse is Action<Inventoriable> onUse)) return;
        stance = new(Stance.Item, default);
        onUse(this);
        if (item.itemType == ItemType.Consum)
        {
            item.stack--;
            if (item.stack <= 0) Inven.Delete(index);
        }
    }
    protected virtual void PickupItem(IItemEntity item, int index)
    {
        if (Inven[index] is IItemEntity oldEntity)
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
        public static readonly ItemData HpPot = new("HPPOT", ItemType.Consum, f => f.Hp += 3);
        public static readonly ItemData Scouter = new("SCOUTR", ItemType.Skill, f => IO.pr(f.Target?.ToString() ?? "No Target to scout."));
        public static readonly ItemData AmuletOfLa = new("AMULLA", ItemType.Equip, f => f.stat.Sol += 20, f => f.stat.Sol -= 20);
        public static readonly ItemData FieryRing = new("FIRING", ItemType.Equip, f => f.stat.Sol += 3, f => f.stat.Sol -= 3);
        public static readonly EquipData2 data = new("LUNRIN", ((s)=>s.stat.RefSol, 3));

        public static readonly ItemData Bag = new(" BAG  ", ItemType.Consum, f => f.Inven.Cap += 2);
        public static readonly ItemData Charge = new("CHARGE", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            f.Move(1);
            f.Move(1);
            f.UseCard(card);
        });
        public static readonly ItemData ShadowAttack = new("SHADOW", ItemType.Skill, f =>
        {
            if (f.SelectCard() is Card card)
            {
                Card newCard = new(card.Lun, card.Sol, card.Con, CardStance.Attack);
                f.UseCard(newCard);
            }
        });
        public static readonly ItemData SNIPE = new("SNIPE ", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            Map.Current.Moveables.TryGet(Player.instance.Pos.x + 2, out Moveable? target);
            Player.instance.Target = target;
            f.UseCard(card);
        });
        public static readonly ItemData Berserk = new("BERSRK", ItemType.Skill, f =>
         {
             Card? card = f.SelectCard();
             if (card?.Stance == CardStance.Attack) f.stance.amount += f.Hp.Max - f.Hp.Cur;
             f.UseCard(card);
         });
        public static readonly ItemData Backstep = new("BKSTEP", ItemType.Skill, f =>
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