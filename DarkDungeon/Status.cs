public class Status
{
    public static Status BasicStatus => new(sol: 1, lun: 1, con: 0);
    public const int MIN = 1;
    public const int BASIC_SIGHT = 1;
    public const int STAT_COUNT = 3;
    public readonly Random rnd = new Random();
    private readonly int[] _data = new int[STAT_COUNT];
    public GamePoint Hp { get; private set; }
    public int Sight { get; private set; } = BASIC_SIGHT;
    public int Level = 1;
    public ExperiencePoint Exp;
    public Status(int sol, int lun, int con)
    {
        Hp = new GamePoint(SolToHp(), GamePointOption.Reserving);
        this[StatName.Sol] = sol;
        this[StatName.Lun] = lun;
        this[StatName.Con] = con;
        Exp = new(this);
        Exp.OnOverflow += new EventHandler(OnLvUp);
    }
    public Status GetDifficultyStat()
    {
        int[] tempData = new int[STAT_COUNT];
        for (int i = 0; i < STAT_COUNT; i++)
        {
            tempData[i] = _data[i].ApplyDifficulty();
        }
        return new(tempData[0], tempData[1], tempData[2]);
    }
    protected int SolToHp() => Level.RoundMult(Rules.LEVEL_TO_BASE_HP_RATE) + this[StatName.Sol];

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
    public int GetRandom(StatName stat, int min = MIN)
    {
        if (stat == StatName.None) return 0;
        int max = this[stat];
        if (min < MIN) min = MIN;
        if (max <= 0) return 0;
        else return rnd.Next(min, max);
    }
    private void OnLvUp(object? sender, EventArgs e)
    {
        Level++;
        Exp.UpdateMax();
        IO.pr($"{Level}레벨이 되었다.", __.emphasis);
        int newHp = SolToHp();
        Hp.Max = newHp;
        //Heal(Hp.Max / 2);
        Heal(Hp.Max);
    }
    public void Damage(int value) => Hp -= value;
    public void Heal(int value) => Hp += value;
    public void AddSight(int value) => Sight += value;
    public void ResetSight() => Sight = BASIC_SIGHT;
    public void GainExp(int value) => Exp = (ExperiencePoint)(Exp + value);
    public override string ToString()
    {
        return $"HP : {Hp}  ^r힘/체력 : {this[StatName.Sol]} ^g집중/민첩 : {this[StatName.Lun]} ^b마력/지능 : {this[StatName.Con]}^/";
    }
}
