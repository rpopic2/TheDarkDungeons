public class Card : IMass
{
    Stance stance;
    public int sol { get; set; }
    public int lun { get; set; }
    public int con { get; set; }
    public Card(int str, int dex, bool silent = true)
    {
        this.sol = str;
        this.lun = dex;
        if (!silent) IO.pr(ToString());
    }
    public void StanceShift()
    {
        if (stance == Stance.Star) return;
        stance = stance == Stance.Attack ? Stance.Defence : Stance.Attack;
    }
    public void ReplaceToStar(int wis)
    {
        this.con = wis;
        stance = Stance.Star;
    }
    public override string ToString()
    {
        if(this == null) return "[     ]";
        if (stance == Stance.Attack) return $"[({sol})/{lun}]";
        else if (stance == Stance.Defence) return $"[{lun}/({sol})]";
        else return $"[{con}*]";
    }
    public Card Draw()
    {
        throw new Exception("Cannot draw card from a card.");
    }
}

public enum Stance
{
    Attack, Defence, Star
}