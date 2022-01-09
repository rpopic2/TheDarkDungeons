public struct CmdTuple
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
    public void Invoke(char key)
        => Acts[Keys.IndexOf(key)]();
}
public static class Extension
{
    public static char ParseKey(this string option)
        => Char.ToLower(option[option.IndexOf('(') + 1]);
    public static char[] ParseKeys(this string[] options)
        => Array.ConvertAll(options, new Converter<string, char>(ParseKey));
}