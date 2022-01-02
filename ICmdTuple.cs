public interface ICmdTuple
{
    List<string> Names { get; }
    List<char> Keys { get; }
    List<Action> Acts { get; }

    bool HasKey(char key);
    void Invoke(char key);
    bool InvokeIfHasKey(char key);
}