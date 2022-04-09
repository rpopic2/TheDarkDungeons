namespace Entities;
public class Inventoriable : Fightable
{
    public Action<Inventoriable> passives = (p) => { };
    public Inventory Inven { get; private set; }
    public Inventoriable(string name, int level, int sol, int lun, int con, int maxHp, int cap) : base(name, level, sol, lun, con, maxHp, cap)
    {
        Inven = new(this, "(맨손)");
    }
    protected void PickupItem(Item item, int index)
    {
        if (index < Inven.Count && Inven[index] is Item old)
        {
            ConsoleKeyInfo keyInfo = IO.rk($"{old.name}이 버려집니다. 계속하시겠습니까?");
            if(keyInfo.Key != IO.CANCELKEY) Inven.Remove(old);
        }
        Inven.Add(item);
    }
    public void SelectSkill(Item item, int index)
    {
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Skill skill) SelectSkill(skill);
        else if (behaviour is Consume consume) SelectConsume(item, consume);
        else IO.rk(behaviour.OnUseOutput);
    }
    public override void OnBeforeTurnEnd()
    {
        passives.Invoke(this);
        base.OnBeforeTurnEnd();
    }

    public void SelectSkill(Skill selected)
    {
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
    public void SelectConsume(Item item, Consume consume)
    {
        SetStance(TokenType.Charge, default);
        consume.behaviour.Invoke(this);
        Inven.Consume(item);
        IO.rk($"{Name}은 {consume.OnUseOutput}");
    }
}