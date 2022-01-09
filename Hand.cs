public struct Hand
{
    private Entity owner;
    public char[] Cur
    {
        get
        {
            char[] curOptions = new char[Cap];
            Array.Copy(IO.NUMERICKEYS, curOptions, Cap);
            return curOptions;
        }
    }
    private Card?[] content;
    public int Cap { get => owner.Cap; }
    public int Count
    {
        get => content.Count(card => card != null);
    }

    public Hand(Entity owner)
    {
        this.owner = owner;
        content = new Card?[owner.Cap];
    }
    public void SetAt(int index, Card card)
    {
        content[index] = card;
    }
    public void Delete(Card card)
    {
        int index = Array.IndexOf(content, card);
        content[index] = null;
    }
    public Card? this[int index]
    {
        get { return content[index]; }
    }
    public void StanceShift(int index)
    {
        Card? card = content[index];
        content[index] = card?.StanceShift();
    }
    public override string ToString()
    {
        string result = "Hand : ";
        foreach (var item in content)
        {
            if (item == null)
            {
                result += "{EMPTY}";
            }
            else
            {
                result += item.ToString();
            }
        }
        return result;
    }
}