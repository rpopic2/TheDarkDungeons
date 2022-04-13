public static class Game
{
    private static Player s_player { get => Player.instance; }
    private const int SPAWNRATE = 10;
    public static int Turn { get; private set; }
    static Game()
    {
        IO.rk($"{s_player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
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
            m.passives.Invoke((Fightable)m); //tryattack
        });
        fights.ForEach(m => m.TryAttack());
        fights.ForEach(m => m.TryDefence());

        fights.ForEach(m =>
        {
            m.OnTurnEnd(); //update target and reset stance
        });
        Map.Current.RemoveAndCreateCorpse();
        if (Turn % SPAWNRATE == 0) Map.Current.Spawn();
        NewTurn();
    }
    public static void NewTurn()
    {
        Turn++;
        Console.Clear();
        IO.pr("History");
        if (s_player.UnderFoot is ISteppable step) IO.pr(step.name + " 위에 서 있다. (spacebar)", __.bottom);
        IO.pr($"턴 : {Turn}  깊이 : {Map.level}\tHP : {s_player.Hp}  Level : {s_player.Level} ({s_player.exp})\t{s_player.tokens}\t 상대 : {s_player.FrontFightable?.tokens}", __.bottom);
        IO.pr(s_player.Inven, __.bottom);
        IO.pr(Map.Current);
    }
}