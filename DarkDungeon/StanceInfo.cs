public class StanceInfo
{
    public int Amount { get; private set; }
    public int Amount2 { get; private set; }
    public Item? CurrentItem { get; private set; }
    public IBehaviour? CurrentBehav { get; private set; }
    public bool IsStun { get; private set; }
    public void Set(Item item, IBehaviour behaviour, int amount = default, int amount2 = default)
    {
        if(IsStun)
        {
            return;
        }
        this.CurrentItem = item;
        this.CurrentBehav = behaviour;
        this.Amount = amount;
        this.Amount2 = amount2;
    }
    public void AddAmount(int value)
    {
        Amount += value;
    }
    public void Reset()
    {
        CurrentItem = null;
        CurrentBehav = null;
        Amount = default;
        Amount2 = default;
    }
    public void SetStun()
    {
        IsStun = true;
    }
    public void ProcessStun()
    {
        Reset();
        IO.rk("스턴 상태!");
        CurrentBehav = Fightable.Stun;
        IsStun = false;
    }
    public override string ToString()
    {
        return $"{Amount}, {Amount2}";
    }
}