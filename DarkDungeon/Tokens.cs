using System.Collections.ObjectModel;

public struct Tokens
{
    public static readonly char[] TokenSymbols = { '(', '[', '<' };
    public static readonly string[] TokenPromptNames = { "(q) (공격)", "(w) [방어]", "(e) <충전>" };

    private List<TokenType> _array;

    public ReadOnlyCollection<TokenType> Content => _array.AsReadOnly();

    public Tokens(int cap)
    {
        _array = new(cap);
    }

    public override string ToString()
    {
        string result = "토큰 :";
        if (Count <= 0) return result + " EMPTY";
        foreach (byte? item in _array)
        {
            if (item is byte value) result += " " + TokenSymbols[value];
        }
        return result;
    }
    public static string ToString(TokenType token)
    {
        return TokenSymbols[(int)token].ToString();
    }
    public void Add(TokenType item)
    {
        if (Count >= _array.Capacity) throw new IndexOutOfRangeException("Your token hand is full. Cannot add more.");
        _array.Add(item);
    }
    public void Remove(TokenType item)
    {
        Remove(item);
    }
    public void RemoveAt(int index)
    {
        _array.RemoveAt(index);
    }
    public int IndexOf(TokenType tokenType)
    {
        return _array.IndexOf(tokenType);
    }
    public bool Contains(TokenType tokenType)
    {
        return _array.Contains(tokenType);
    }
    public TokenType? this[int index]
    {
        get
        {
            if (index >= _array.Count) return null;
            return _array[index];
        }
    }
    public int Count => _array.Count;
    public bool IsFull => Count >= _array.Capacity;

    public TokenType? TryUse(TokenType token)
    {
        if (_array.IndexOf(token) != -1)
        {
            Remove(token);
            return token;
        }
        else
        {
            return null;
        }
    }
}

public enum TokenType
{
    Offence, Defence, Charge
}