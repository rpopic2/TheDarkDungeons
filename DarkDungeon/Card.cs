public readonly struct Card
{
    private readonly char[] STATCHARS = {'s', 'L', '*'};
    private const string OFFENSIVE = "()";
    private const string DEFENSIVE = "[]";


    public readonly int value;
    public readonly Stats stat;
    public readonly bool isOffence;
    private readonly string visual;

    public Card(int value, Stats stat, bool isOffence)
    {
        this.value = value;
        this.stat = stat;
        this.isOffence = isOffence;
        
        char statChar = STATCHARS[((int)stat)];
        visual = isOffence ? OFFENSIVE : DEFENSIVE;
        visual = visual.Insert(1, $"{value}{statChar}");
    }

    public override string ToString() => visual;
    public string ToStringPrivate() => isOffence ? OFFENSIVE : DEFENSIVE;

    public static Card StanceShift(Card card) => new Card(card.value, card.stat, !card.isOffence);
}