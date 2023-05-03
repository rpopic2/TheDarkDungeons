public record SkillOld(string Name, StanceName Stance, StatName StatDepend, DamageType damageType, string OnUseOutput, Action<Creature> Behaviour) : IBehaviour, IEnergyConsume
{
    public static readonly string[] parenthesis = { "  ", "[]", "()", "<>", "<>", "{}" };
    public override string ToString()
    {
        string result = parenthesis[(int)damageType];
        string tempName = Name.Insert(0, GetColor());
        tempName += "^/";
        result = result.Insert(1, tempName);
        return result;

        string GetColor() => StatDepend switch
        {
            StatName.Sol => "^r",
            StatName.Lun => "^g",
            StatName.Con => "^b",
            _ => "^/"
        };
    }
}
public record Charge(string Name, StatName StatDepend, DamageType damageType, string OnUseOutput, Action<Creature> Behaviour) : IBehaviour, IEnergyConsume
{
    public StanceName Stance { get; init; } = StanceName.Charge;
    public override string ToString()
    {
        string result = SkillOld.parenthesis[(int)damageType];
        string tempName = Name.Insert(0, GetColor());
        tempName += "^/";
        result = result.Insert(1, tempName);
        return result;

        string GetColor() => StatDepend switch
        {
            StatName.Sol => "^r",
            StatName.Lun => "^g",
            StatName.Con => "^b",
            _ => "^/"
        };
    }
}
public record NonTokenSkill(string Name, StanceName Stance, string OnUseOutput, Action<Creature, int, int> NonTokenBehav, Action<Creature> Behaviour) : IBehaviour;

public record Consume(string Name, StanceName Stance, string OnUseOutput, Action<Creature> Behaviour) : IBehaviour
{
    public override string ToString()
    {
        return Name;
    }
}

public record WearEffect(string Name, StanceName Stance, string OnUseOutput, Action<Creature> Behaviour, Action<Creature> OnTakeOff) : IBehaviour
{
    public override string ToString() => Name;
}

public record Passive(string Name, StanceName Stance, string OnUseOutput, Action<Creature> Behaviour) : IBehaviour
{
    public override string ToString() => Name;
}

public interface IBehaviour
{
    public StanceName Stance { get; }
    public string Name { get; }
    public string OnUseOutput { get; }
    public Action<Creature> Behaviour { get; }
}
public interface IEnergyConsume : IBehaviour
{
    public StatName StatDepend { get; }
}
