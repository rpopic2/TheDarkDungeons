public struct Hand
{
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
    public int Cap { get; private set; }
    public int Count
    {
        get => content.Count(card => card != null);
    }

    public Hand(int cap = 3)
    {
        Cap = cap;
        content = new Card[cap];
    }
    private void _Pickup(int index, Card card, bool silent = false)
    {
        content[index] = card;
        if (!silent) IO.pr(ToString());
    }

    public void Pickup(Card card, bool silent = false)
    {
        IO.pr("You've found a card." + card);
        this.prh();
        bool cancel = IO.sel(Player.hand.Cur, out int index, out char key);
        if(cancel) return;
        _Pickup(index, card, silent);
        IO.del();
        IO.del();
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
    public void StanceShift(int index, bool silent = false)
    {
        content[index]?.StanceShift();
        if (!silent) IO.pr(ToString());
    }
    public override string ToString()
    {
        string result = "Hand : ";
        foreach (var item in content)
        {
            if (item == null)
            {
                result += "[EMPTY]";
            }
            else
            {
                result += item.ToString();
            }
        }
        return result;
    }
}