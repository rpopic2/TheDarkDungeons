public readonly struct Token
{
    public static readonly char[] Tokens = {'(', '[', '<'};
    public readonly TokenType value;
    public Token(TokenType value)
    {
        this.value = value;
    }
    public override string ToString()
    {
        return Tokens[(int)value].ToString();
    }
}

public enum TokenType
{
    Offence, Defence, Charge
}