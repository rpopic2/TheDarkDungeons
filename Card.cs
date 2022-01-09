public struct Card : IMass
{
    public Stance Stance { get; private set; }

    public int sol { get; private set; }
    public int lun { get; private set; }
    public int con { get; private set; }

    public Card(int sol, int lun, int con, Stance stance)
    {
        this.sol = sol;
        this.lun = lun;
        this.con = con;
        this.Stance = stance;
    }
    public Card StanceShift()
    {
        if (Stance == Stance.Star) return this;
        Stance = Stance == Stance.Attack ? Stance.Defence : Stance.Attack;
        return this;
    }
    public void ReplaceToStar(int wis)
    {
        this.con = wis;
        Stance = Stance.Star;
    }
    public override string ToString()
    {
        if (Stance == Stance.Attack) return $"<({sol})/{lun}>";
        else if (Stance == Stance.Defence) return $"[({lun})/{sol}]";
        else return $"[{con}*]";
    }
}