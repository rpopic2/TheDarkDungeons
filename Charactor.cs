class Charactor : Entity
{
    public string charName = "Michael";
    public ClassName charClass;
    private Random rnd = new Random();
    int exp = 0;
    const float lvCurve = 1.25f;
    const int lvMultiplier = 10;

    public int MaxExp
    {
        get { return (int)MathF.Floor(lv * lvMultiplier * lvCurve); }
    }

    public void PrintStats()
    {
        string result = $"Name : {charName}\tClass : {charClass.ToString()}\tLevel : {lv}\tExperience : {exp}\nHp : {maxHp}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
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
        ClassSwitch(() => { sol += 2; }, () => { lun += 2; }, () => { con += 2; });
        Console.WriteLine("\nLevel up!");
    }
    public Card Draw()
    {
        Card card = new Card(rnd.Next(sol), rnd.Next(lun));
        IO.pr("You've found a card." + card);
        return card;
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
