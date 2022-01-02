public struct CmdTuple : ICmdTuple
{
    public List<string> Names { get; private set; }
    public List<char> Keys { get; private set; }
    public List<Action> Acts { get; private set; }
    public CmdTuple()
    {
        Names = new List<string>();
        Keys = new List<char>();
        Acts = new List<Action>();
    }
    public void Add(string name, Action act)
    {
        Names.Add(name);
        Keys.Add(name.ParseKey());
        Acts.Add(act);
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
public static class Extension
{
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);
    public static List<char> ParseKeys(this string[] options)
    {
        List<char> result = new List<char>();
        foreach (string item in options)
        {
            result.Add(item.ParseKey());
        }
        return result;
    }
}