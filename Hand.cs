public class Hand
{
    private string[] options = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
    private int cap;
    private Card[] content;
    public int Count
    {
        get
        {
            return content.Count(card => card != null);
        }
    }
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
        int index = selh();
        Pickup(index, card, silent);
    }

    public int selh()
    {
        IO.pr(ToString());
        string[] curOptions = new string[cap];
        for (int i = 0; i < cap; i++)
        {
            curOptions[i] = options[i];
        }
        return IO.sel(curOptions);
    }

    public override string ToString()
    {
        string result = string.Join(' ', content.ToList());
        return result == string.Empty ? "Empty" : result;
    }

    internal void StanceShift()
    {
        throw new NotImplementedException();
    }
}