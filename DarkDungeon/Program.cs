﻿global using Entities;
public class Program
{
    public const string VERSION = "0.6.050522";
    public static Program instance = default!;
    private static Player s_player { get => Player.instance; }
    public static int Turn { get; private set; }
    public static void Main()
    {
        instance = new Program();
        do
        {
            ElaspeTurn();
        } while (s_player.IsAlive);
        IO.pr(s_player.ToString());
        IO.pr($"{s_player.Name}은 여기에 잠들었다...");
        IO.rk();
    }
    public Program()
    {
        instance = this;
        Console.Clear();
        IO.pr("The Dungeon of the Mine " + VERSION);
        CreatePlayer();
        Map.NewMap();
        Console.Clear();
        IO.rk($"{s_player.Name}은 횃불에 의지한 채 동굴 속으로 걸어 들어갔다.");
        IO.Redraw();
        IO.pr("\n?을 눌러 도움말 표시.");
    }
    private static void ElaspeTurn()
    {
        var fights = Map.Current.Creatures;
        //onbeforeturn
        fights.ForEach(f =>
        {
            f.OnBeforeTurn();
        });
        //onturn
        var firsts = from f in fights where f.CurAction.CurrentBehav?.Stance == StanceName.Charge select f;
        var lasts = fights.Except(firsts);
        foreach (Creature item in firsts) item.OnTurn();
        foreach (Creature item in lasts) item.OnTurn();
        //onturnend
        fights.ForEach(m =>
        {
            m.OnTurnEnd(); //update target and reset stance, onturnend
        });

        Map.Current.ReplaceToCorpse();
        if (Map.Current.DoSpawnMobs && Turn % Rules.Spawnrate == 0) Map.Current.SpawnRandom();
        Turn++;
        if(Map.Current.DoLoadNewMap) Map.NewMap();
        IO.Redraw();
    }
    private void CreatePlayer()
    {
        IO.rk("Press any key to start...");

        string name;
        do
        {
            IO.pr("캐릭터의 이름은?...");
            name = Console.ReadLine() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);
    ClassSelect:
        IO.pr($"{name}의 직업은?...");
        int classIndex = 0;
        string[] classes = new string[] { "^r (전사)^/", "^g (암살자)^/", "^b (마법사)^/" };
        IO.sel(classes, 0, out classIndex, out bool cancel, out _, out _); ;
        string? classString = classes[classIndex];

        switch (classIndex)
        {
            case 0:
                IO.pr($"{classString} : 난이도 쉬움. (초보 추천) 근접 거리에서 용맹히 싸우는 직업이다.");
                break;
            case 1:
                IO.pr($"{classString} : 난이도 보통. (초보 비추천) 적이 저항하지 못하고 본인이 당했는지도 모르게끔 암살한다.");
                break;
            case 2:
                IO.pr($"{classString} : 난이도 어려움. (초보 비추천) 창의력을 발휘하여야 하는 강력한 마법을 쓰는 직업이다.");
                break;
            default:
                IO.pr("자유 직업을 선택하였습니다.");
                break;
        }
    Confirm:
        ConsoleKeyInfo keyInfo = IO.rk("스페이스바 : 확인, x : 취소");
        if (keyInfo.Key.IsCancel())
        {
            IO.del(2);
            goto ClassSelect;
        }
        else if (!keyInfo.Key.IsOK()) goto Confirm;
        else IO.del(2);

        if (classIndex != -1) IO.pr(classString);
        Player player = Player._instance = new Player(name);
        switch (classIndex)
        {
            case 0:
                player.Inven.Add(Creature.sword);
                player.Inven.Add(Creature.shield);
                player.Inven.Add(Creature.torch);
                player.GainEnergy(3);
                break;
            case 1:
                player.Inven.Add(Creature.dagger);
                player.Inven.Add(Creature.bow);
                player.Inven.Add(Creature.arrow);
                player.Inven.Add(Creature.arrow);
                player.Inven.Add(Creature.torch);
                player.Inven.Add(Creature.assBareHand);
                player.GainEnergy(3);
                break;
            case 2:
                player.Inven.Add(Creature.staff);
                player.Inven.Add(Creature.magicBook);
                player.Inven.Add(Creature.torch);
                player.GainEnergy(3);
                break;
            default:
                player.SelectStartItem();
                player.GainEnergy(3);
                break;
        }
        IO.pr("초보자 도움말 : 능력치는 하나에 집중 투자하는것이 더 쉽습니다.");
        player.SelectPickupStat(3);
    }
}