public struct Card : IMass
{
    public CardStance Stance { get; private set; }
    public readonly int Level { get; }
    public readonly int Sol { get; }
    public readonly int Lun { get; }
    public readonly int Con { get; }

    public Card(int sol, int lun, int con, CardStance stance)
    {
        Sol = sol;
        Lun = lun;
        Con = con;
        Level = (sol + lun + con) / 3;
        Stance = stance;
    }
    public Card StanceShift()
    {
        if (Stance == CardStance.Star) return this;
        Stance = Stance == CardStance.Attack ? CardStance.Dodge : CardStance.Attack;
        return this;
    }

    internal Card Exile()
    {
        Stance = CardStance.Star;
        return this;
    }

    public override string ToString()
    {
        if (Stance == CardStance.Attack) return $"<({Sol})/{Lun}>";
        else if (Stance == CardStance.Dodge) return $"[({Lun})/{Sol}]";
        else return $"[{Con}*]";
    }

}