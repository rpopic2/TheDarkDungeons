namespace Entities;
public class Inventoriable : Fightable
{
    public Inventory<NewItem> NewInven {get; private set;}
    public Inventoriable(string name, ClassName className, int level, int sol, int lun, int con, int maxHp, int cap) : base(name, className, level, sol, lun, con, maxHp, cap)
    {
        NewInven = new(3, "(맨손)");
        //RegisterItem(11, TorchData.data);
    }
    protected void NewPickupItem(NewItem item, int index)
    {
        NewInven[index] = item;
    }
}