public class Bolt : ItemNew, IStackable
{
    public Bolt(int stack = 1)
    {
        Stack = stack;
    }

    public override string Name => "석궁 볼트";

    public int Stack
    {
        get => Stack;
        set
        {
            if (value < 1) throw new ArgumentOutOfRangeException("Stack cannot be less than 1.");
            Stack = value;
        }
    }
}