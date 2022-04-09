public record Skill(string Name, TokenType TokenType, StatName statName, string OnUseOutput, Action<Fightable>? behaviour = null) : IBehaviour
{
    public static readonly string[] parenthesis = { "()", "[]", "<>" };
    public override string ToString()
    {
        string result = parenthesis[(int)TokenType];
        result = result.Insert(1, Name);
        return result;
    }
}

public record Consume(string Name, string OnUseOutput, Action<Fightable> behaviour) : IBehaviour
{
    public override string ToString()
    {
        return Name;
    }
}

public record WearEffect(string Name, string OnUseOutput, Action<Inventoriable> wear, Action<Inventoriable> takeOff) : IBehaviour
{
    public override string ToString() => Name;
}

public record Passive(string Name, string OnUseOutput, Action<Inventoriable> actionEveryTurn) : IBehaviour
{
    public override string ToString() => Name;
}

public interface IBehaviour
{
    public string Name { get; }
    public string OnUseOutput { get; }
}