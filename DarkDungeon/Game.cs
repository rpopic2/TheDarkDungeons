public static class Game
{
    private static Player Player { get => Player.instance; }
    public static event EventHandler? OnTurnEnd;

    public static int Turn { get; private set; }
    static Game()
    {
        Map.NewMap();
        Player.StartItem();
    }
    internal static void ElaspeTurn()
    {
        Array.ForEach(Map.Current.Moveables, m =>
        {
            if (m is Monster mon) mon.DoTurn();
        });
        Array.ForEach(Map.Current.Moveables, m =>
        {
            if (m is Fightable f) f.OnBeforeTurnEnd();
        });
        Array.ForEach(Map.Current.Moveables, m =>
        {
            if (m is Fightable f) f.OnTurnEnd();
        });

        OnTurnEnd?.Invoke(null, EventArgs.Empty);
        if (IO.printCount == 3) IO.del(2);
        if (Turn % 5 == 0) Map.Current.Spawn();

        NewTurn();
    }
    public static void NewTurn()
    {
        IO.printCount = 0;
        Turn++;
        IO.pr($"\nTurn : {Turn}\tDungeon Level : {Map.level}");
    }
}