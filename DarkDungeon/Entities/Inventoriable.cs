namespace Entities;
public class Inventoriable : Fightable
{
    public Inventory<Item> Inven { get; private set; }
    public Inventoriable(string name, int level, int sol, int lun, int con, int maxHp, int cap) : base(name, level, sol, lun, con, maxHp, cap)
    {
        Inven = new(cap, "(맨손)");
    }
    protected void NewPickupItem(Item item, int index)
    {
        Inven[index] = item;
    }
    public void SelectSkill(Item item, int index)
    {
        Skill? selected = item.skills[index];
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = SetStance(token, selected.statName);
            string s = $"{Name}은 {selected.OnUseOutput} ({amount})";
            if (tempCharge > 0) s += ($"+({tempCharge})");
            if (selected.statName == StatName.Con) tempCharge += amount;
            IO.rk(s);
        }
        else
        {
            IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
        }
    }
}