public class CurrentAction
{
    public int Amount { get; private set; }
    public int Amount2 { get; private set; }
    public Item? CurrentItem { get; private set; }
    public IBehaviour? CurrentBehav { get; private set; }
    public int Stun { get; private set; }
    public int Poison { get; private set; }
    private Creature _owner;
    public CurrentAction(Creature owner)
    {
        _owner = owner;
    }
    public void Set(Item item, IBehaviour behaviour, int amount = default, int amount2 = default)
    {
        if (Stun > 0)
        {
            ProcessStun();
            return;
        }
        this.CurrentItem = item;
        this.CurrentBehav = behaviour;
        this.Amount = amount;
        this.Amount2 = amount2;
        bool isPrepend = behaviour.Stance == StanceName.Charge;
        Map.Current.AddToOnTurn(_owner.OnTurn, isPrepend);
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
    public void SetStun(int value) => Stun += value;
    public void SetPoison(int value) => Poison += value;
    public void ProcessStun()
    {
        Reset();
        IO.rk($"{_owner.Name}은 스턴 상태이다!");
        CurrentBehav = Creature.Stun;
        Stun--;
    }
    public void ProcessPoison()
    {
        _owner.Stat.Damage(1);
        Poison--;
    }
    public override string ToString()
    {
        return $"{CurrentItem?.Name}, {Amount}, {Amount2}";
    }
}