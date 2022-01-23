public class Inventory
{
    private Item?[] content;

    public Inventory(int cap)
    {
        this.Cap = cap;
        content = new Item?[cap];
    }

    public int Cap { get; private set; }

    public override string ToString()
    {
        string result = "Inventory : ";
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
    public Item? this[int index]
    {
        get => content[index];
        set => content[index] = value;
    }
}