public struct Stack
{
    public Stack(int v)
    {
        if (v < 1) throw new ArgumentOutOfRangeException("Stack cannot be less than 1.");
        _value = v;
    }

    private int _value;

    public static implicit operator Stack(int v) => new Stack(v);
}