namespace Entities;
public class Inventoriable : Fightable
{
    public Inventory<Item> Inven {get; private set;}
    public Inventoriable(string name, int level, int sol, int lun, int con, int maxHp, int cap) : base(name, level, sol, lun, con, maxHp, cap)
    {
        Inven = new(cap, "(맨손)");
    }
    protected void NewPickupItem(Item item, int index)
    {
        Inven[index] = item;
    }
}