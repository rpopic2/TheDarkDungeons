using System.Collections.ObjectModel;

public struct Tokens
{
    public static readonly char[] TokenSymbols = { '(', '[', '<' };
    public static readonly string[] TokenPromptNames = { "(공격", "[방어", "<충전" };

    private List<byte?> _array;

    public ReadOnlyCollection<byte?> Content => _array.AsReadOnly();

    public Tokens(int cap)
    {
        _array = new(cap);
    }

    public override string ToString()
    {
        string result = "토큰 :";
        if(Count <= 0) return result + " EMPTY";
        foreach (byte? item in _array)
        {
            if (item is byte value) result += " " + TokenSymbols[value];
        }
        return result;
    }
    public void Add(byte item)
    {
        if (Count >= _array.Capacity) throw new IndexOutOfRangeException("Your token hand is full. Cannot add more.");
        _array.Add(item);
    }
    public void Add(TokenType item)
    {
        Add((byte)item);
    }
    public void Remove(byte item)
    {
        _array.Remove(item);
    }
    public void Remove(TokenType item)
    {
        Remove((byte)item);
    }
    public void RemoveAt(int index)
    {
        _array.RemoveAt(index);
    }
    public byte? this[int index]
    {
        get => _array[index];
    }
    public int Count => _array.Count((i) => i is not null);
    public bool IsFull => Count >= _array.Capacity;
}

public enum TokenType
{
    Offence, Defence, Charge
}