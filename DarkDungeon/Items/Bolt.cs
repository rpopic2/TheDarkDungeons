public class Bolt : ItemNew, IStackable
{
    public Bolt(int stack = 1)
    {
        this.stack = stack;
    }

    public override string Name => "석궁 볼트";

    public Stack stack { get; set; }
}