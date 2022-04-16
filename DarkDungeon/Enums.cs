public enum GamePointOption
{
    Stacking, Reserving
}
public enum Facing
{
    Right, Left
}
public enum StanceName
{
    None, Offence, Defence, Charge
}
public enum ItemType
{
    Equip, Consume
}
[Flags]
public enum __
{
    emphasis = 1,
    newline = 2,
    ///<summary>This flag calls Console.Write() by default. To add a line terminator, add a newline flag.</summary>
    bottom = 4,
    fullinven = 8,
    write = 16,
    ///<summary>^b=Blue, ^r=Red, ^g=Green ^/ =reset</summary>
    color_on = 32,
}
public enum DamageType
{
    None, Normal, Slash, Thrust, Magic
}