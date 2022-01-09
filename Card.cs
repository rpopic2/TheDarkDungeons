public struct Card : IMass
{
    public Stance Stance { get; private set; }
    public int Sol { get; private set; }
    public int Lun { get; private set; }
    public int Con { get; private set; }

    public Card(int sol, int lun, int con, Stance stance)
    {
        Sol = sol;
        Lun = lun;
        Con = con;
        Stance = stance;
    }
    public Card StanceShift()
    {
        if (Stance == Stance.Star) return this;
        Stance = Stance == Stance.Attack ? Stance.Defence : Stance.Attack;
        return this;
    }
    public void ReplaceToStar(int wis)
    {
        this.Con = wis;
        Stance = Stance.Star;
    }
    public override string ToString()
    {
        if (Stance == Stance.Attack) return $"<({Sol})/{Lun}>";
        else if (Stance == Stance.Defence) return $"[({Lun})/{Sol}]";
        else return $"[{Con}*]";
    }
}