public class Inventoriable : Fightable
{
    public Inventory<IItem?> Inven { get; private set; }

    public Inventoriable(string name, ClassName className, int level, int sol, int lun, int con, int maxHp, int cap) : base(name, className, level, sol, lun, con, maxHp, cap)
    {
        Inven = new Inventory<IItem?>(3, "Inventory");
    }

    public void UseInven(int index)
    {
        if (!(Inven[index] is IItem item)) return;
        if (!(item.onUse is Func<Inventoriable, bool> onUse)) return;
        if (item.stack <= 0) return;
        bool success = onUse(this);
        if (!success)
        {
            stance = new(default, default);
            return;
        }
        if (item is not Equip)
        {
            if (item.stack > 0) item.stack--;
            else if (item.itemType == ItemType.Consum) Inven.Delete(index);
        }
    }
    protected void PickupItem(IItem item, int index)
    {
        if (Inven[index] is IItem oldEntity)
        {
            if (oldEntity.itemType == ItemType.Consum)
            {
                if (oldEntity.abv == item.abv)
                {
                    oldEntity.stack++;
                    return;
                }
            }
            else if (oldEntity is Equip oldEquip) oldEquip.onUse.Invoke(false);
        }
        if (item.itemType == ItemType.Skill) item.stack = Player.skillMax;
        Inven[index] = item;
        if (item is Equip equip) equip.onUse.Invoke(true);
    }
    public static class ConsumeDb
    {
        public static readonly ItemData HpPot = new("HPPOT", ItemType.Consum, f =>
        {
            f.Hp += 3; return true;
        });
        public static readonly ItemData Bag = new(" BAG  ", ItemType.Consum, f =>
        {
            f.Inven.Cap += 2; return true;
        });
    }
    public static class SkillDb
    {
        public static readonly ItemData Scouter = new("SCOUTR", ItemType.Skill, f =>
        {
            IO.pr(f.Target?.ToString() ?? "No Target to scout.");
            return f.Target is not null;
        });
        public static readonly ItemData Charge = new("CHARGE", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            if (card is not Card card2) return false;
            f.Move(1);
            f.Move(1);
            f.UpdateTarget();
            f.UseCard(card2);
            return true;
        });
        public static readonly ItemData ShadowAttack = new("SHADOW", ItemType.Skill, f =>
        {
            if (f.SelectCard() is Card card)
            {
                Card newCard = new(card.Lun, card.Sol, card.Con, CardStance.Attack);
                f.UseCard(newCard);
                return true;
            }
            return false;
        });
        public static readonly ItemData SNIPE = new("SNIPE ", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            if (card is not Card card2) return false;
            Map.Current.Moveables.TryGet(Player.instance.Pos.x + 2, out Moveable? target);
            if (target is null) return false;
            Player.instance.Target = target;
            f.UseCard(card2);
            return card is not null;
        });
        public static readonly ItemData Berserk = new("BERSRK", ItemType.Skill, f =>
        {
            Card? card = f.SelectCard();
            if (card is not Card card2) return false;
            if (card2.Stance == CardStance.Attack) f.stance.amount += f.Hp.Max - f.Hp.Cur;
            else return false;
            f.UseCard(card2);
            return true;
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
                    if (obj == MapSymb.portal) return false;
                    mov.Move(2 * mov.Pos.FrontMul);
                    mov.Move(1 * mov.Pos.BackMul);
                    return true;
                }
            }
            return false;
        });
    }
}