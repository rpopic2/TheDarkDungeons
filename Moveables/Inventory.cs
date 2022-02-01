public class Inventory<T>
{
    private T?[] content;
    private int cap;
    public readonly string name;

    public Inventory(int cap, string name)
    {
        this.Cap = cap;
        content = new T?[cap];
        this.name = name;
    }
    public int Cap
    {
        get => cap;
        set
        {
            if (value < 3) value = 3;
            if (value > 10) value = 10;
            cap = value;
            Array.Resize(ref content, value);
        }
    }
    public T? GetFirst()
        => content.First(card => card != null);
    public int Count => content.Count(item => item != null);

    public override string ToString()
    {
        string result = $"{name} : ";
        foreach (T? item in content)
        {
            if (item == null) result += "{EMPTY}";
            else result += item.ToString();
        }
        return result;
    }
    public void Delete(T item)
    {
        int index = Array.IndexOf(content, item);
        if (index != -1) content[index] = default(T?);
    }
    public void Delete(int index)
    {
        if (content[index] is not null) content[index] = default(T?);
    }
    public T? this[int index]
    {
        get => content[index];
        set => content[index] = value;
    }
}