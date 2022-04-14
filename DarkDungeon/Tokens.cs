using System.Collections;
using System.Collections.ObjectModel;

public struct Tokens : ICollection<TokenType>
{
    public static readonly char[] TokenSymbols = { '(', '[', '<' };
    public static readonly string[] TokenPromptNames = { "(q) (공격)", "(w) [방어]", "(e) <충전>" };

    private List<TokenType> _content;

    public ReadOnlyCollection<TokenType> Content => _content.AsReadOnly();

    public Tokens(int cap)
    {
        _content = new(cap);
    }

    public override string ToString()
    {
        string result = "토큰 :";
        if (Count <= 0) return result + " EMPTY";
        foreach (byte? item in _content)
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
        if (Count >= _content.Capacity) throw new IndexOutOfRangeException("Your token hand is full. Cannot add more.");
        _content.Add(item);
    }
    public void Remove(TokenType item)
    {
        _content.Remove(item);
    }
    public void RemoveAt(int index)
    {
        _content.RemoveAt(index);
    }
    public int IndexOf(TokenType tokenType)
    {
        return _content.IndexOf(tokenType);
    }
    public bool Contains(TokenType tokenType)
    {
        return _content.Contains(tokenType);
    }
    public TokenType? this[int index]
    {
        get
        {
            if (index >= _content.Count) return null;
            return _content[index];
        }
    }
    public int Count => _content.Count;
    public bool IsFull => Count >= _content.Capacity;

    public bool IsReadOnly => ((ICollection<TokenType>)_content).IsReadOnly;

    public TokenType? TryUse(TokenType token)
    {
        if (_content.IndexOf(token) != -1)
        {
            Remove(token);
            return token;
        }
        else
        {
            return null;
        }
    }

    public void Clear()
    {
        ((ICollection<TokenType>)_content).Clear();
    }

    public void CopyTo(TokenType[] array, int arrayIndex)
    {
        ((ICollection<TokenType>)_content).CopyTo(array, arrayIndex);
    }

    bool ICollection<TokenType>.Remove(TokenType item)
    {
        return ((ICollection<TokenType>)_content).Remove(item);
    }

    public IEnumerator<TokenType> GetEnumerator()
    {
        return ((IEnumerable<TokenType>)_content).GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_content).GetEnumerator();
    }
}

public enum TokenType
{
    Offence, Defence, Charge
}