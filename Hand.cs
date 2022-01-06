public struct Hand
{
    public static Hand Player = Program.player.Hand;
    public static char[] PlayerCur = Program.player.Hand.CurOption;
    public char[] CurOption
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
    public void Pickup(int index, Card card, bool silent = false)
    {
        content[index] = card;
        if (!silent) IO.pr(ToString());
    }

    public void Pickup(Card card, bool silent = false)
    {
        this.prh();
        IO.sel(Hand.PlayerCur, out int index, out char key);
        Pickup(index, card, silent);
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