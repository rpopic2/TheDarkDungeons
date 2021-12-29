using System.Collections.Generic;
using System.Numerics;
public enum ClassName
{
    Warrrior, Assassin, Mage
}
public class Charactor
{

    public string charName = "Michael";
    public ClassName charClass;
    List<Card> hand = new List<Card>();
    Random rnd = new Random();
    public int hp = 10;
    int lv = 0;
    int exp = 0;
    const float lvCurve = 1.25f;
    const int lvMultiplier = 10;
    public int str = 4;
    public int dex = 4;
    public int wis = 4;

    public int MaxExp
    {
        get { return (int)MathF.Floor(lv * lvMultiplier * lvCurve); }
    }
    public string Hands
    {
        get
        {
            string result = String.Join(' ', hand);
            return "\nHand : " + result ?? "Empty";
        }
    }

    public void PrintStats()
    {
        string result = $"Name : {charName}\tClass : {charClass.ToString()}\tLevel : {lv}\tExperience : {exp}\nHp : {hp}\tStrength : {str}\tDexterity : {dex}\tWisdom : {wis}";
        Console.WriteLine(result);
    }

    public void GainExp(int exp)
    {
        this.exp += exp;
        if (exp >= MaxExp)
        {
            exp -= MaxExp;
            LvUp();
        }
    }

    public Card Draw()
    {
        Card card = new Card(rnd.Next(str), rnd.Next(dex));
        hand.Add(card);
        return card;
    }
    public void FlipStanceAt(int index)
    {
        hand[index].FlipStance();
    }

    void LvUp()
    {
        exp = 0;
        lv++;
        ClassSwitch(() => { str += 2; }, () => { dex += 2; }, () => { wis += 2; });
        Console.WriteLine("\nLevel up!");
    }
    public void ClassSwitch(Action warrior, Action assassin, Action mage)
    {
        switch (charClass)
        {
            case ClassName.Warrrior:
                warrior();
                break;
            case ClassName.Assassin:
                assassin();
                break;
            case ClassName.Mage:
                mage();
                break;
        }
    }
}
