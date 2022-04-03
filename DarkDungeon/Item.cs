public record Item(string name, Skill[] skills)
{
    public static readonly Item bareHand = new("(맨손)", new Skill[] {
        new("주먹질", TokenType.Offence, StatName.Sol, "주먹을 휘둘렀다."),
        new("구르기", TokenType.Defence, StatName.Lun, "옆으로 굴렀다.")
        });
    public static readonly Item sword = new("(검)", new Skill[] {
        new("베기", TokenType.Offence, StatName.Sol, "칼을 휘둘러 앞을 베었다."),
        new("칼로막기", TokenType.Defence, StatName.Lun, "칼로 막기 자세를 취했다.")
        });
    public static readonly Item holySword = new("(광란의 신성검)", new Skill[] {
        new("베기", TokenType.Offence, StatName.Sol, "칼을 휘둘러 앞을 베었다."),
        new("광란의기도", TokenType.Charge, StatName.Con, "미친 듯이 기도하였고 칼이 빛나기 시작했다.")
        });
    public static readonly Item staff = new("지팡이", new Skill[] {
        new("휘두르기", TokenType.Offence, StatName.Sol, "지팡이를 휘둘렀다."),
        new("별빛부름", TokenType.Charge, StatName.Con, "신비한 별빛을 불러내어 지팡이를 감쌌다.")
        });
    public override string ToString()
    {
        return name;
    }
}