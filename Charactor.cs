using System.Collections.Generic;
public enum ClassName
{
    Warrrior, Assassin, Mage
}
public class Charactor
{

    public string charName = "Michael";
    public ClassName charClass;
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

    public void PrintStats()
    {
        string result = $"Name : {charName} | Class : {charClass.ToString()} | Level : {lv} | Experience : {exp}\nStrength : | {str} Dexterity : | {dex} Wisdom : {wis}";
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

    void LvUp()
    {
        exp = 0;
        lv++;
        ClassSwitch(()=>{str+=2;}, ()=>{dex+=2;}, ()=>{wis+=2;});
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
