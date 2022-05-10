public class Program
{
    public const string VERSION = "0.6.100522";
    public static Program instance = default!;
    private static Player s_player { get => Player.instance; }
    public static Action? OnTurn;
    public static void Main()
    {
        instance = new Program();
        do
        {
            OnTurn?.Invoke();
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
    private void CreatePlayer()
    {
        IO.rk("Press any key to start...");
        string name = ChooseName();
    ClassSelect:
        IO.pr($"{name}의 직업은?...");
        int classIndex = 0;
        string[] classes = new string[] { "^r (전사)^/", "^g (암살자)^/", "^b (마법사)^/" };
        IO.sel(classes, 0, out classIndex, out bool cancel, out _, out _); ;
        string classString = classes.ElementAtOrDefault(classIndex) ?? "자유전직";
    Confirm:
        ShowClassDescription();
        ConsoleKeyInfo keyInfo = IO.rk("스페이스바 : 확인, x : 취소");
        if (keyInfo.Key.IsCancel())
        {
            IO.del(3);
            goto ClassSelect;
        }
        else if (!keyInfo.Key.IsOK()) goto Confirm;
        else IO.del(2);

        if (classIndex != -1) IO.pr(classString);
        Player player = Player._instance = new Player(name);
        AddStartItems(classIndex);
        IO.pr("초보자 도움말 : 능력치는 하나에 집중 투자하는것이 더 쉽습니다.");
        player.SelectPickupStat(3);

        void ShowClassDescription()
        {
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
                    IO.pr(classString);
                    break;
            }
        }
    }
    private string ChooseName()
    {
        string name;
        do
        {
            IO.pr("캐릭터의 이름은?...");
            name = Console.ReadLine() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);
        return name;
    }
    private void AddStartItems(int classIndex)
    {
        Inventory playerInven = s_player.Inven;
        switch (classIndex)
        {
            case 0:
                playerInven.Add(Creature.sword);
                playerInven.Add(Creature.shield);
                playerInven.Add(Creature.torch);
                s_player.GainEnergy(3);
                break;
            case 1:
                playerInven.Add(Creature.dagger);
                playerInven.Add(Creature.bow);
                playerInven.Add(Creature.arrow);
                playerInven.Add(Creature.arrow);
                playerInven.Add(Creature.torch);
                playerInven.Add(Creature.assBareHand);
                s_player.GainEnergy(3);
                break;
            case 2:
                playerInven.Add(Creature.staff);
                playerInven.Add(Creature.magicBook);
                playerInven.Add(Creature.torch);
                s_player.GainEnergy(3);
                break;
            default:
                s_player.SelectStartItem();
                s_player.GainEnergy(3);
                break;
        }
    }
}