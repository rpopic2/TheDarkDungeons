namespace Item;
public class BareHand : ItemBase
{
    public override string Name => "맨손";
    public override List<Action> Skills { get; init; } = new();
}