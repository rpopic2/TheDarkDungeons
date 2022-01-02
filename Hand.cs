public struct Hand
{
    private Card[] content;
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
    public void Pickup(int index, Card card, bool silent = false)
    {
        content[index] = card;
        if (!silent) IO.pr(ToString());
    }

    public void Pickup(Card card, bool silent = false)
    {
        Pickup(this.selh(), card, silent);
    }
    public void StanceShift(int index, bool silent = false)
    {
        content[index].StanceShift();
        if (!silent) IO.pr(ToString());
    }
    public void StanceShift(bool silent = false)
    {
        StanceShift(this.selc(), silent);
    }

    public override string ToString()
    {
        string result = "";
        foreach (var item in content)
        {
            if (item == null)
            {
                result += "[     ]";
            }
            else
            {
                result += item.ToString();
            }
        }
        return result;
    }
}