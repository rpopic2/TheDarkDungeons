public record Skill(string Name, StanceName Stance, StatName statName, DamageType damageType, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public static readonly string[] parenthesis = { "  ", "[]", "()", "<>", "{}" };
    public override string ToString()
    {
        string result = parenthesis[(int)damageType];
        string tempName = Name.Insert(0, GetColor());
        tempName += "^/";
        result = result.Insert(1, tempName);
        return result;

        string GetColor() => statName switch
        {
            StatName.Sol => "^r",
            StatName.Lun => "^g",
            StatName.Con => "^b",
            _ => "^/"
        };
    }
}
public record Charge(string Name, StatName statName, DamageType damageType, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public StanceName Stance { get; init; } = StanceName.Charge;
    public override string ToString()
    {
        string result = Skill.parenthesis[(int)damageType];
        string tempName = Name.Insert(0, GetColor());
        tempName += "^/";
        result = result.Insert(1, tempName);
        return result;

        string GetColor() => statName switch
        {
            StatName.Sol => "^r",
            StatName.Lun => "^g",
            StatName.Con => "^b",
            _ => "^/"
        };
    }
}
public record NonTokenSkill(string Name, StanceName Stance, string OnUseOutput, Action<Fightable, int, int> NonTokenBehav, Action<Fightable> Behaviour) : IBehaviour;

public record Consume(string Name, StanceName Stance, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public override string ToString()
    {
        return Name;
    }
}

public record WearEffect(string Name, StanceName Stance, string OnUseOutput, Action<Fightable> Behaviour, Action<Fightable> OnTakeOff) : IBehaviour
{
    public override string ToString() => Name;
}

public record Passive(string Name, StanceName Stance, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public override string ToString() => Name;
}

public interface IBehaviour
{
    public StanceName Stance { get; }
    public string Name { get; }
    public string OnUseOutput { get; }
    public Action<Fightable> Behaviour { get; }
}