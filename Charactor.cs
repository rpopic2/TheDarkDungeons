public class Charactor : Entity
{
    public string charName = "Michael";
    public ClassName charClass;
    private Random rnd = new Random();
    public Exp exp;
    public Charactor()
    {
        exp = new Exp(this);
    }

    public void PrintStats()
    {
        string result = $"Name : {charName}\tClass : {charClass.ToString()}\tLevel : {lv}\tExperience : {exp}\nHp : {hp.Max}\tStrength : {sol}\tDexterity : {lun}\tWisdom : {con}";
        Console.WriteLine(result);
    }
    public void OnLvUp()
    {
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
