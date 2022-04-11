public static class Extensions
{
    public static bool TryGet<T>(this T[] source, int index, out T? obj)
    {
        obj = default(T?);
        if (index < 0 || index >= source.Length) return false;
        else if (source[index] is null) return false;
        else
        {
            obj = source[index];
            return true;
        }
    }
    public static int Distance(this Position pos1, Position pos2) => pos2.x - pos1.x;

    public static Facing Flip(this Facing facing)
        => facing == Facing.Right ? Facing.Left : Facing.Right;

    public static int RoundMult(this int @base, float mult)
    => (int)MathF.Round(@base * mult);
    public static int FloorMult(this int @base, float mult)
    => (int)MathF.Floor(@base * mult);
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);
    public static char[] ParseKeys(this string[] options)
        => Array.ConvertAll(options, new Converter<string, char>(ParseKey));
    public static StanceName ToStance(this TokenType token) => token switch
    {
        TokenType.Offence => StanceName.Offence,
        TokenType.Defence => StanceName.Defence,
        TokenType.Charge => StanceName.Charge,
        _ => StanceName.None
    };
    public static string ToFString(this Array value, string comment = "선택 :")
    {
        string printResult = comment + " /";
        foreach (var item in value)
        {
            printResult += $" {item} /";
        }
        return printResult;
    }
    public static int ToVul(this int damage)
    {
        int result = (int)MathF.Round(damage * Rules.vulMulp);
        return result == damage ? ++result : result;
    }
    public static bool IsEnemy(this Fightable p1, Fightable p2)
    {
        if (p1 is Player && p2 is Monster) return true;
        if (p1 is Monster && p2 is Player) return true;
        return false;
    }
}