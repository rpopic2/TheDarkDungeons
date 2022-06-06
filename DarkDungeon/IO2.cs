public static class IO2
{
    private static Player s_player => Player.instance;
    private static ItemNew? selectedItem;
    public static void Press(char v)
    {
        int index = IO.ITEMKEYS1.IndexOf(v);
        if (selectedItem is null) selectedItem = s_player.GetItemAt(index);
        else
        {
            Program.OnTurn += selectedItem.Skills[index];
            Program.ElaspeTurn();
        }
    }
}