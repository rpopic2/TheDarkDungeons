using System.Numerics;
public enum Stance
{
    attack, defence, star
}
public class Card
{
    Stance stance;
    int str;
    int dex;
    int wis;
    public Card(int str, int dex)
    {
        this.str = str;
        this.dex = dex;
    }
    public void FlipStance()
    {
        if (stance == Stance.attack)
        {
            stance = Stance.defence;
        }
    }
    public void ReplaceToStar(int wis)
    {
        this.wis = wis;
        stance = Stance.star;
    }
    public override string ToString()
    {
        if (stance == Stance.attack)
        {
            return $"[({str})/{dex}]";
        }
        else if (stance == Stance.defence)
        {
            return $"[{dex}/({str})]";
        }else{
            return $"[{wis}*]";
        }
    }
}