public static class IO2
{
    private static Player s_player => Player.instance;
    public static void Press(char v)
    {
        int index = IO.ITEMKEYS1.IndexOf(v);
        // ItemNew item = s_player.GetItemAt(index);
        Program.OnTurn += s_player.GetItem<Item.ShadowDagger>().Pierce;
        Program.ElaspeTurn();
    }
}