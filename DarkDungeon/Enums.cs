public enum GamePointOption
{
    Stacking, Reserving
}
public enum StanceName
{
    None, Offence, Defence, Charge
}
public enum ItemType
{
    Equip, Consume
}
public enum StatName
{
    Sol, Lun, Con, None
}

[Flags]
public enum __
{
    emphasis = 1,
    newline = 2,
    ///<summary>This flag calls Console.Write() by default. To add a line terminator, add a newline flag.</summary>
    bottom = 4,
    fullinven = 8,
    ///<summary>^b=Blue, ^r=Red, ^g=Green ^/ =reset</summary>
}
public enum DamageType
{
    None, Normal, Slash, Throw, Thrust, Magic
}
