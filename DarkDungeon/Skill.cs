public record Skill(string Name, TokenType TokenType, Stats stats, string OnUseOutput)
{
    public static readonly Skill[] bardHand = { new("주먹질", TokenType.Offence, Stats.Sol, "주먹을 휘둘렀다."), new("구르기", TokenType.Defence, Stats.Lun, "옆으로 굴렀다.") };
    public static readonly Skill[] sword = { new("베기", TokenType.Offence, Stats.Sol, "칼을 휘둘러 앞을 베었다."), new("칼로막기", TokenType.Defence, Stats.Lun, "칼로 막기 자세를 취했다.") };

    private readonly string[] parenthesis = { "()", "[]", "<>" };
    public override string ToString()
    {
        string result = parenthesis[(int)TokenType];
        result = result.Insert(1, Name);
        return result;
    }
}