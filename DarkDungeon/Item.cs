public record Item(string name, ItemType itemtType, IBehaviour[] skills)
{
    public static readonly Item bareHand = new("(맨손)", ItemType.Equip, new Skill[] {
        new("주먹질", TokenType.Offence, StatName.Sol, "주먹을 휘둘렀다."),
        new("구르기", TokenType.Defence, StatName.Lun, "옆으로 굴렀다.")
        });
    public static readonly Item sword = new("(검)", ItemType.Equip, new Skill[] {
        new("베기", TokenType.Offence, StatName.Sol, "칼을 휘둘러 앞을 베었다."),
        new("칼로막기", TokenType.Defence, StatName.Lun, "칼로 막기 자세를 취했다.")
        });
    public static readonly Item holySword = new("(광란의 신성검)", ItemType.Equip, new Skill[] {
        new("베기", TokenType.Offence, StatName.Sol, "칼을 휘둘러 앞을 베었다."),
        new("광란의기도", TokenType.Charge, StatName.Con, "미친 듯이 기도하였고 칼이 빛나기 시작했다.")
        });
    public static readonly Item staff = new("(지팡이)", ItemType.Equip, new Skill[] {
        new("휘두르기", TokenType.Offence, StatName.Sol, "지팡이를 휘둘렀다."),
        new("별빛부름", TokenType.Charge, StatName.Con, "신비한 별빛을 불러내어 지팡이를 감쌌다.")
        });
    public static readonly Item dagger = new("(단검)", ItemType.Equip, new Skill[] {
        new("찌르기", TokenType.Offence, StatName.Sol, "단검으로 적을 찔렀다."),
        new("투검", TokenType.Offence, StatName.Lun, "적을 향해 단검을 던졌다.")
    });
    public static readonly Item bat = new("(박쥐)", ItemType.Equip, new Skill[] {
        new("들이박기", TokenType.Offence, StatName.Lun, "갑자기 당신의 얼굴로 날아들어 부딪혔다!"),
        new("구르기", TokenType.Defence, StatName.Lun, "가벼운 날개짓으로 옆으로 피했다.")
        });
    public static readonly Item tearOfLun = new("<달의 눈물>", ItemType.Consume, new IBehaviour[]{
        new Consume("사용한다", "포션을 상처 부위에 떨어뜨렸고, 이윽고 상처가 씻은 듯이 아물었다.", (p)=>{Player.instance.Hp += 3;})
    });
    public override string ToString()
    {
        return name;
    }
}