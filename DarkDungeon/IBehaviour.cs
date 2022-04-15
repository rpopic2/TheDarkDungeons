public record Skill(string Name, StanceName Stance, TokenType TokenType, StatName statName, DamageType damageType, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public static readonly string[] parenthesis = { "()", "[]", "<>" };
    public override string ToString()
    {
        string result = parenthesis[(int)TokenType];
        result = result.Insert(1, Name);
        return result;
    }
}
public record NonTokenSkill(string Name, StanceName Stance, string OnUseOutput, Action<Fightable, int, int> nonTokenBehav, Action<Fightable> Behaviour) : IBehaviour;

public record Consume(string Name, StanceName Stance, string OnUseOutput, Action<Fightable> Behaviour) : IBehaviour
{
    public override string ToString()
    {
        return Name;
    }
}

public record WearEffect(string Name, StanceName Stance, string OnUseOutput, Action<Fightable> Behaviour, Action<Fightable> takeOff) : IBehaviour
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