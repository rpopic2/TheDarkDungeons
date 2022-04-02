public record Skill(string Name, TokenType TokenType, Stats stats, string OnUseOutput)
{
    private readonly string[] parenthesis = {"()", "[]", "<>"};
    public override string ToString()
    {
        string result = parenthesis[(int)TokenType];
        result = result.Insert(1, Name);
        return result;
    }
}