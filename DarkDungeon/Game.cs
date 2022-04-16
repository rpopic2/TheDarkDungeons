public static class Game
{
    private static Player s_player { get => Player.instance; }
    private static int s_spawnrate = 10;
    public static int Turn { get; private set; }
    static Game()
    {
        IO.rk($"{s_player.Name}은 광산 입구로 들어갔다. 계속 들어가다 보니 빛이 희미해졌다.");
    }
    internal static void ElaspeTurn()
    {
        var fights = Map.Current.Fightables;
        fights.ForEach(f =>
        {
            if (f.Stance.IsStun)  f.Stance.ProcessStun();
            else f.DoTurn();
        });

        fights.ForEach(m =>
        {
            m.passives.Invoke((Fightable)m); //passives
        });

        fights.ForEach(m => m.InvokeBehaviour());

        fights.ForEach(m =>
        {
            m.OnTurnEnd(); //update target and reset stance
        });
        Map.Current.RemoveAndCreateCorpse();
        if (Map.Current.SpawnMobs && Turn % s_spawnrate == 0) Map.Current.Spawn();
        NewTurn();
    }
    public static void NewTurn()
    {
        Turn++;
        IO.DrawScreen();
    }
}