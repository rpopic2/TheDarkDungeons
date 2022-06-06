namespace Item;
public class BareHand : ItemNew
{
    public override string Name => "맨손";
    public override List<Action> Skills { get; init; } = new();
}