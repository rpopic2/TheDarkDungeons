public struct Card : IMass
{
    public Stance Stance { get; private set; }
    public readonly int Level { get; }
    public readonly int Sol { get; }
    public readonly int Lun { get; }
    public readonly int Con { get; }

    public Card(int sol, int lun, int con, Stance stance)
    {
        Sol = sol;
        Lun = lun;
        Con = con;
        Level = (sol + lun + con) / 3;
        Stance = stance;
    }
    public Card StanceShift()
    {
        if (Stance == Stance.Star) return this;
        Stance = Stance == Stance.Attack ? Stance.Defence : Stance.Attack;
        return this;
    }

    internal Card Exile()
    {
        Stance = Stance.Star;
        return this;
    }

    public override string ToString()
    {
        if (Stance == Stance.Attack) return $"<({Sol})/{Lun}>";
        else if (Stance == Stance.Defence) return $"[({Lun})/{Sol}]";
        else return $"[{Con}*]";
    }

}