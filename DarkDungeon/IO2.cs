public static class IO2
{
    private static Player s_player => Player.instance;
    public static ItemBase? SelectedItem;
    public static void Press(char v)
    {
        int index = IO.ITEMKEYS1.IndexOf(v);
        if (index == -1) return;
        IO.pr(s_player.InvenToString);
        if (SelectedItem is null) SelectedItem = s_player.GetItemAt(index);
        else
        {
            Program.OnTurn += SelectedItem.Skills[index];
            Program.ElaspeTurn();
            SelectedItem = null;
        }
    }
}