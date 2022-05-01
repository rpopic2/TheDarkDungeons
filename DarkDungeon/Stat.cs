public class Status
{
    public const int MIN = 1;
    public const int BASIC_SIGHT = 1;
    public readonly Random rnd = new Random();
    private readonly int[] _data = new int[3];
    public GamePoint Hp { get; private set; }
    public int Sight { get; private set; } = BASIC_SIGHT;
    public Status(int sol, int lun, int con)
    {
        Hp = new GamePoint(SolToHp(), GamePointOption.Reserving);
        this[StatName.Sol] = sol;
        this[StatName.Lun] = lun;
        this[StatName.Con] = con;
    }
    protected int SolToHp() => 1 + this[StatName.Sol].RoundMult(0.8f);

    public int this[StatName index]
    {
        get => _data[(int)index];
        set
        {
            _data[(int)index] = value;
            if (index == StatName.Sol)
            {
                if (value > 0) Hp.IncreaseMax(SolToHp() - Hp.Max);
                if (value < 0) Hp.DecreaseMax(Hp.Max - SolToHp());
            }
        }
    }
    public int GetRandom(StatName stat)
    {
        if (stat == StatName.None) return 0;
        int max = this[stat];
        if (max <= 0) return 0;
        else return rnd.Next(MIN, max);
    }
    public void Damage(int value)
    {
        Hp -= value;
    }
    public void Heal(int value)
    {
        Hp += value;
    }
    public void AddSight(int value)
    {
        Sight += value;
    }
    public void ResetSight()
    {
        Sight = BASIC_SIGHT;
    }
    public override string ToString()
    {
        return $"HP : {Hp}  ^r힘/체력 : {this[StatName.Sol]} ^g집중/민첩 : {this[StatName.Lun]} ^b마력/지능 : {this[StatName.Con]}^/";
    }
}