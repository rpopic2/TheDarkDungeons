public class Hand
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
    private void _Pickup(int index, Card card, bool silent = false)
    {
        content[index] = card;
        if (!silent) IO.pr(ToString());
    }

    public void PlayerPickup(Card card, bool silent = false)
    {
        if(owner is not Player) throw new Exception("Target is not player.");
        IO.pr("You've found a card." + card);
        IO.pr(this);
        IO.sel(owner.Hand.Cur, out int index, out char key, out bool cancel);
        if (cancel)
        {
            IO.del();
            return;
        }
        _Pickup(index, card, silent);
        IO.del(2);
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