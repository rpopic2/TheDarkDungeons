namespace Item;
public class Bolt : ItemNew, IStackable
{
    public Bolt(int stack = 1)
    {
        this.Stack = stack;
    }

    public override string Name => "석궁 볼트";
    public override List<Action> Skills { get; init; } = new();

    public Stack Stack { get; set; }
}