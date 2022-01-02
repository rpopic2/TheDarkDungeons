public struct CmdTupleCard : ICmdTuple
{
    public List<string> Names { get; }

    public List<char> Keys { get; }

    public List<Action> Acts { get; }
    public CmdTupleCard()
    {
        Names = new List<string>(new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" });
        Keys = new List<char>(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' });
        Acts = new List<Action>();
    }

    public bool HasKey(char key)
    {
        return Keys.Contains(key);
    }

    public void Invoke(char key)
        => Acts[Keys.IndexOf(key)]();
    

    public bool InvokeIfHasKey(char key)
    {
        bool result = HasKey(key);
        if (result) Invoke(key);
        return result;
    }
}