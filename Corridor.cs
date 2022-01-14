public readonly struct Corridor
{
    public readonly char[] data;
    public Corridor(int column)
    {
        data = new char[column];
        for (int i = 0; i < column; i++)
        {
            data[i] = '-';
        }
    }
    public override string ToString()
    {
        return string.Join(" ", data);
    }
}