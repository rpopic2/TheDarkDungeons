public class Card : Mass
{
    public Stance Stance { get; private set; }

    public Card(int str, int dex, bool silent = true)
    {
        this.sol = str;
        this.lun = dex;
        if (!silent) IO.pr(ToString());
    }
    public void StanceShift()
    {
        if (Stance == Stance.Star) return;
        Stance = Stance == Stance.Attack ? Stance.Defence : Stance.Attack;
    }
    public void ReplaceToStar(int wis)
    {
        this.con = wis;
        Stance = Stance.Star;
    }
    public override string ToString()
    {
        if(this == null) return "[     ]";
        if (Stance == Stance.Attack) return $"[({sol})/{lun}]";
        else if (Stance == Stance.Defence) return $"[{lun}/({sol})]";
        else return $"[{con}*]";
    }
    public override Card Draw()
    {
        throw new Exception("Cannot draw card from a card.");
    }
}