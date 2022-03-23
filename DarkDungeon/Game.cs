public static class Game
{
    private static Player Player { get => Player.instance; }
    private const int SpawnRate = 10;
    public static event EventHandler? OnTurnEnd;

    public static int Turn { get; private set; }
    static Game()
    {
        Map.NewMap();
        Player.StartItem();
    }
    internal static void ElaspeTurn()
    {
        var temp = (from mov in Map.Current.Moveables where mov is not null select mov).ToArray();
        Array.ForEach(temp, m =>
        {
            if (m is Monster mon) mon.DoTurn();
        });
        Array.ForEach(temp, m =>
        {
            if (m is Fightable f) f.OnBeforeTurnEnd();
        });
        Array.ForEach(temp, m =>
        {
            if (m is Fightable f) f.OnTurnEnd();
        });

        OnTurnEnd?.Invoke(null, EventArgs.Empty);
        if (IO.printCount == 3) IO.del(2);
        if (Turn % SpawnRate == 0) Map.Current.Spawn();

        NewTurn();
    }
    public static void NewTurn()
    {
        IO.printCount = 0;
        Turn++;
        IO.pr($"\nTurn : {Turn}\tDungeon Level : {Map.level}\tHP : {Player.instance.Hp}");
    }
}