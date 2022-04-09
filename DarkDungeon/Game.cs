public static class Game
{
    private static Player _Player { get => Player.instance; }
    private const int SpawnRate = 10;
    public static event EventHandler? OnTurnEnd; //unused

    public static int Turn { get; private set; }
    static Game()
    {
        IO.rk($"{_Player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
    }
    internal static void ElaspeTurn()
    {
        var fights = Map.Current.Fightables;
        fights.ForEach(m =>
        {
            if (m is Monster mon) mon.DoTurn(); //mob ai
        });
        fights.ForEach(m =>
        {
            if (m is Fightable f) f.OnBeforeFight(); //tryattack
        });
        fights.ForEach(m=>m.TryAttack());
        fights.ForEach(m=>m.TryDefence());
        fights.ForEach(m =>
        {
            if (m is Fightable f) f.OnTurnEnd(); //update target and reset stance
        });
        Map.Current.RemoveCorpse();
        OnTurnEnd?.Invoke(null, EventArgs.Empty);
        if (Turn % SpawnRate == 0) Map.Current.Spawn();
        NewTurn();
    }
    public static void NewTurn()
    {
        IO.printCount = 0;
        Turn++;
        Console.Clear();
        IO.pr("History");
        IO.prb($"\n턴 : {Turn}  깊이 : {Map.level}\tHP : {_Player.Hp}  Level : {_Player.Level} ({_Player.exp})\t{_Player.tokens}\t 상대 : {((Fightable?)_Player.Target)?.tokens}");
        IO.prb(_Player.Inven);
        IO.pr(Map.Current);
    }
}