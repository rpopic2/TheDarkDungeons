public readonly struct Map
{
    public readonly char[] data;
    public Map(int column)
    {
        data = new char[column];
        for (int i = 0; i < column; i++)
        {
            data[i] = '-';
        }
        data[0] = '@';
    }
    public override string ToString()
    {
        return string.Join(" ", data);
    }
}