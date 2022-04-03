public enum ClassName
{
    Warrior, Assassin, Mage
}


public enum CardStance
{
    Offence, Defence,
}
public enum GamePointOption
{
    Stacking, Reserving
}
public enum Facing
{
    Front, Back
}
public enum Stance
{
    None, Offence, Defence, Charge
}
public enum ItemType
{
    Equip, Skill, Consum
}
public enum It
{
    HpPot, Bag, Scouter, Charge, ShadowAttack, Snipe, Berserk, Backstep, LunarRing, AmuletOfLa, FieryRing, Torch
}

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
        => facing == Facing.Front ? Facing.Back : Facing.Front;

    public static int RoundMult(this int @base, float mult)
    => (int)MathF.Round(@base * mult);
    public static int FloorMult(this int @base, float mult)
    => (int)MathF.Floor(@base * mult);
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);
    public static char[] ParseKeys(this string[] options)
        => Array.ConvertAll(options, new Converter<string, char>(ParseKey));
    public static Stance ToStance(this TokenType token) =>  token switch{
        TokenType.Offence => Stance.Offence,
        TokenType.Defence => Stance.Defence,
        TokenType.Charge => Stance.Charge,
        _ => Stance.None
    };
}