public class Hand
{
    //private Fightable owner;
    private Card?[] content;
    public int Cap { get; private set; }
    public int Count => content.Count(card => card != null);

    public Hand(int cap)
    {
        content = new Card?[cap];
        UpdateHandCap(cap);
    }
    public void UpdateHandCap(int cap)
    {
        if (cap <= 0 || cap > 10) throw new ArgumentException("cap is out of index");
        if(Cap == cap) return;
        Cap = cap;
        Array.Resize(ref content, Cap);
    }
    public void SetAt(int index, Card card)
    {
        content[index] = card;
    }
    public Card GetAt(int index)
    {
        return content[index] ?? throw new Exception();
    }
    public Card GetFirst()
        => content.First(card => card != null) ?? throw new Exception();
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
    public void Exile(int index)
    {
        Card? card = content[index];
        content[index] = card?.Exile();
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