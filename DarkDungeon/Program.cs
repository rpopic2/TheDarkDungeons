global using IO = DarkDungeon.IO;
public class Program
{
    public const string VERSION = "0.6.040622";
    private static Player s_player { get => Player.instance; }
    public static Action? OnTurn;
    public static void Main()
    {
        Console.CancelKeyPress += (e, e2) => {Environment.Exit(1);};
        Program instance = new Program();
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
        Map.NewMap();
        CreatePlayer();
        Console.Clear();
        IO.rk($"{s_player.Name}은 횃불에 의지한 채 동굴 속으로 걸어 들어갔다.");
        IO.Redraw();
        IO.pr("\n?을 눌러 도움말 표시.");
    }
    public static void ElaspeTurn() => OnTurn?.Invoke();
    private void CreatePlayer()
    {
        string name = ChooseName();
        IO.pr($"{name}의 직업은?...");
        int classIndex = ChooseClass();
        Player player = Player._instance = new Player(name, classIndex);
        AddStartItems(classIndex);
    }
    private int ChooseClass()
    {
    ClassSelect:
        int classIndex = 0;
        string[] classes = new string[] { "^r (전사)^/", "^g (암살자)^/", "^b (마법사)^/" };
        IO.sel(classes, out classIndex, 0/*, "(x로 자유전직) "*/);
        string className = classes.ElementAtOrDefault(classIndex) ?? "자유전직";
    Confirm:
        string classDesc = GetClassDesc(classIndex);
        IO.pr($"{className} : {classDesc}");
        Status stat = Status.BasicStatus;
        if (classIndex != -1) stat[(StatName)classIndex] += 3;
        IO.pr($"기본 스탯 : {stat}");
        ConsoleKeyInfo keyInfo = IO.rk("스페이스바 : 확인, x : 취소");
        if (keyInfo.Key.IsCancel())
        {
            IO.del(2);
            goto ClassSelect;
        }
        else if (!keyInfo.Key.IsOK()) goto Confirm;
        IO.del(1);
        return classIndex;
    }
    private string ChooseName()
    {
        if (!IO.IsInteractive) return "TestPlayer";
        string name;
        do
        {
            IO.pr("캐릭터의 이름은?...");
            name = Console.ReadLine() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);
        return name;
    }
    private string GetClassDesc(int index)
    {
        string desc;
        switch (index)
        {
            case 0:
                desc = "근접 거리에서 용맹히 싸우는 직업이다. 높은 체력을 지니고 있어 안정적이다.";
                break;
            case 1:
                desc = "적이 저항하지 못하고 본인이 당했는지도 모르게끔 암살한다. 적과 맞붙었을 때 약하며 체력이 적어 한번에 죽을 수 있다.";
                break;
            case 2:
                desc = "창의력을 발휘하여 강력한 마법을 쓰는 직업이다. 여러가지 스킬 조합이 가능하지만 쓰기 어렵다.";
                break;
            default:
                desc = "초보 비추천. 스탯과 아이템을 당신이 직접 고를 수 있다.";
                break;
        }
        return desc;
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
                break;
            case 1:
                playerInven.Add(Creature.dagger);
                playerInven.Add(Creature.bow);
                playerInven.Add(Creature.arrow);
                playerInven.Add(Creature.arrow);
                playerInven.Add(Creature.torch);
                playerInven.Add(Creature.assBareHand);
                break;
            case 2:
                playerInven.Add(Creature.staff);
                playerInven.Add(Creature.magicBook);
                playerInven.Add(Creature.torch);
                break;
            default:
                s_player.SelectStartItem();
                break;
        }
    }
}