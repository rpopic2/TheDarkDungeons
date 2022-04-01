public readonly struct Token
{
    public static readonly char[] Tokens = {'(', '[', '<'};
    public static readonly string[] TokenNames = {"공격", "방어", "충전"};
    public readonly TokenType value;
    public Token(TokenType value)
    {
        this.value = value;
    }
    public char ToChar()
    {
        return Tokens[(int)value];
    }
    public override string ToString()
    {
        return $"{ToChar()}{TokenNames[(int)value]} ";
    }
}

public enum TokenType
{
    Offence, Defence, Charge
}