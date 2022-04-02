public record Item(string name, Skill[] skills)
{
    public static readonly Item bardHand = new("(맨손)", new Skill[] {
        new("주먹질", TokenType.Offence, Stats.Sol, "주먹을 휘둘렀다."),
        new("구르기", TokenType.Defence, Stats.Lun, "옆으로 굴렀다.")
        });
    public static readonly Item sword = new("(검)", new Skill[] {
        new("베기", TokenType.Offence, Stats.Sol, "칼을 휘둘러 앞을 베었다."),
        new("칼로막기", TokenType.Defence, Stats.Lun, "칼로 막기 자세를 취했다.")
        });
    public override string ToString()
    {
        return name;
    }
}