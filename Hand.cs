public class Hand
{
    private int cap;
    private Card[] content;
    public int Count
    {
        get
        {
            return content.Count(card => card != null);
        }
    }

    public int Cap { get => cap; }

    public Hand(int cap = 3)
    {
        this.cap = cap;
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
    public void StanceShift()
    {
        content[this.selc()].StanceShift();
    }

    public override string ToString()
    {
        string result = string.Join(' ', content.ToList());
        return result == string.Empty ? "Empty" : result;
    }
}