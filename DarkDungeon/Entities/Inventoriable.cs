namespace Entities;
public partial class Inventoriable : Fightable
{
    public Action<Inventoriable> passives = (p) => { };
    public Inventoriable(string name, int level, int sol, int lun, int con, int maxHp, int cap, Position pos) : base(name, level, sol, lun, con, maxHp, cap, pos)
    {

    }
    public void SelectBehaviour(Item item, int index)
    {
        if (Stance.Stance != StanceName.None) throw new Exception("스탠스가 None이 아닌데 새 동작을 선택했습니다. 한 턴에 두 동작을 할 수 없습니다.");
        IBehaviour behaviour = item.skills[index];
        if (behaviour is Skill skill) SelectSkill(item, skill);
        else if (behaviour is Consume consume) SelectConsume(item, consume);
        else if (behaviour is Passive) IO.rk(behaviour.OnUseOutput);
        else throw new Exception("등록되지 않은 행동 종류입니다.");

    }
    public void SelectBasicBehaviour(int index, int x, int y)
    {
        IBehaviour behaviour = basicActions.skills[index];
        if (behaviour is NonTokenSkill nonToken)
        {
            string output = behaviour.OnUseOutput;
            if(output != string.Empty)IO.rk(Name + output);
            stance.Set(StanceName.Charge, default);
            nonToken.behaviour.Invoke(this, x, y);
        }
    }

    public override void OnBeforeFight()
    {
        passives.Invoke(this);
        base.OnBeforeFight();
    }
    private void SelectSkill(Item item, Skill selected)
    {
        TokenType? tokenTry = tokens.TryUse(selected.TokenType);
        if (tokenTry is TokenType token)
        {
            int amount = SetStance(token, selected.statName);
            string s = $"{Name} {selected.OnUseOutput} ({amount})";
            int mcharge = Inven.GetMeta(item).magicCharge;
            if (mcharge > 0) s += ($"+({mcharge})");
            if (selected.statName == StatName.Con) Inven.GetMeta(item).magicCharge += amount;
            currentBehav = selected.behaviour;
            currentItem = item;
            IO.rk(s);
        }
        else
        {
            IO.rk($"{Tokens.TokenSymbols[(int)selected.TokenType]} 토큰이 없습니다.");
        }
    }
    private void SelectConsume(Item item, Consume consume)
    {
        SetStance(TokenType.Charge, default);
        IO.rk($"{Name} {consume.OnUseOutput}");
        consume.behaviour.Invoke(this);
        Inven.Consume(item);
    }
}