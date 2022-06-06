public class Bolt : ItemNew, IStackable
{
    public Bolt(int stack = 1)
    {
        this.Stack = stack;
    }

    public override string Name => "석궁 볼트";

    public Stack Stack { get; set; }
}