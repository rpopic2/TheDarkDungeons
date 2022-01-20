public enum ClassName
{
    Warrior, Assassin, Mage
}

public enum Stance
{
    Attack, Defence, Star
}
public enum GamePointOption
{
    Stacking, Reserving
}
public enum KeyArrow
{
    Cancel, UpArrow, DownArrow, LeftArrow, RightArrow
}
public enum Facing
{
    Front, Back
}

public static class Extensions
{
    public static bool TryGet<T>(this T[] source, int index, out T? obj)
    {
        obj = default(T);
        if (index < 0 || index >= source.Length) return false;
        else if (source[index] is null) return false;
        else
        {
            obj = source[index];
            return true;
        }
    }
    public static Facing Flip(this Facing facing)
        => facing == Facing.Front ? Facing.Back : Facing.Front;

    public static int RoundMult(this int @base, float mult)
    => (int)MathF.Round(@base * mult);
    public static int FloorMult(this int @base, float mult)
    => (int)MathF.Floor(@base * mult);
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);
    public static char[] ParseKeys(this string[] options)
        => Array.ConvertAll(options, new Converter<string, char>(ParseKey));
}