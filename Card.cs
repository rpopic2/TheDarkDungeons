using System.Numerics;

public class Card : IMass
{
    Stance stance;
    public int sol {get; set;}
    public int lun {get; set;}
    public int con {get; set;}
    public Card(int str, int dex)
    {
        this.sol = str;
        this.lun = dex;
    }
    public void StanceShift()
    {
        if (stance == Stance.star) return;
        stance = stance == Stance.attack ? Stance.defence : Stance.attack;
    }
    public void ReplaceToStar(int wis)
    {
        this.con = wis;
        stance = Stance.star;
    }
    public override string ToString()
    {
        if (stance == Stance.attack)
        {
            return $"[({sol})/{lun}]";
        }
        else if (stance == Stance.defence)
        {
            return $"[{lun}/({sol})]";
        }
        else
        {
            return $"[{con}*]";
        }
    }
}

public enum Stance
{
    attack, defence, star
}