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
    public static int Distance(this Position pos1, Position pos2) => (int)MathF.Abs(pos2.x - pos1.x);

    public static Facing Flip(this Facing facing)
        => facing == Facing.Right ? Facing.Left : Facing.Right;
    public static int ApplyDifficulty(this int stat) => (int)(stat + MathF.Floor(Rules.LEVEL_DIFFICULTY * Map.Depth));

    public static int RoundMult(this int @base, float mult)
    => (int)MathF.Round(@base * mult);
    public static int FloorMult(this int @base, float mult)
    => (int)MathF.Floor(@base * mult);
    public static string ToFString(this Array value)
    {
        string printResult = string.Empty;
        if(value.Length <= 0) printResult += "아무것도 없다.";
        for (int i = 0; i < value.Length; i++)
        {
            printResult += value.GetValue(i)?.ToString()?.AppendKeyName(i);
        }
        return printResult;
    }
    public static string AppendKeyName(this string value, int index)
    {
        return $"{IO.ITEMKEYS1[index]}{value} ";
    }
    public static int ToVul(this int damage)
    {
        int result = (int)MathF.Round(damage * Rules.vulMulp);
        return result == damage ? ++result : result;
    }
    public static int ToUnVul(this int damage)
    {
        int result = (int)MathF.Round(damage / Rules.vulMulp);
        return result == damage ? --result : result;
    }
    public static bool IsEnemy(this Fightable p1, Fightable p2)
    {
        if (p1 is Player && p2 is Monster) return true;
        if (p1 is Monster && p2 is Player) return true;
        return false;
    }
    public static char[] NewFilledArray(int length, char fill)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = fill;
        }
        return result;
    }
    public static bool IsOK(this ConsoleKey key)
    {
        return key == ConsoleKey.Spacebar || key == ConsoleKey.Enter;
    }
    public static bool IsCancel(this ConsoleKey key)
    {
        return key == ConsoleKey.X || key == ConsoleKey.OemMinus || key == ConsoleKey.Escape;
    }
}