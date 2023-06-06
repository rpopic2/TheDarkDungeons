public static class PlayerCreation {
    private static Player s_player => Player.instance;

    public static void CreatePlayerPrompt()
    {
        string name = ChooseNamePrompt();
        IO.pr($"{name}의 직업은?...");
        int classIndex = ChooseClassPrompt();
        Player._instance = new Player(name, classIndex);
        AddStartItems(classIndex);
    }

    private static int ChooseClassPrompt()
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
        IO.gs?.pr($"<class_info>{className}:{classDesc}:{stat}");
        IO.pr("스페이스바 : 확인, x : 취소");
        ConsoleKeyInfo keyInfo = IO.rk();
        if (keyInfo.Key.IsCancel() || keyInfo.KeyChar == 'x')
        {
            IO.del(2);
            goto ClassSelect;
        }
        else if (!keyInfo.Key.IsOK()) goto Confirm;
        IO.del(1);
        return classIndex;
    }

    private static string ChooseNamePrompt()
    {
        if (!IO.IsInteractive) return "TestPlayer";
        string name;
        do
        {
            IO.pr("캐릭터의 이름은?...");
            name = IO.rl() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);
        return name;
    }

    private static string GetClassDesc(int index)
    {
        string desc;
        switch (index)
        {
            case 0:
                desc = "근접 거리에서 용맹히 싸우는 직업이다. 방패의 패리로 적을 기절시키며 제압할 수 있다. 또한 높은 체력을 지니고 있어 안정적이다.";
                break;
            case 1:
                desc = "적이 저항하지 못하고 본인이 당했는지도 모르게끔 암살한다. 체력이 적어 자칫 한번에 죽을 수 있다.";
                break;
            case 2:
                desc = "창의력을 발휘하여 강력한 마법을 쓰는 직업이다. 수많은 스킬 조합이 가능하지만 운용 난이도는 높은 편이다.";
                break;
            default:
                desc = "초보 비추천. 스탯과 아이템을 당신이 직접 고를 수 있다.";
                break;
        }
        return desc;
    }

    private static void AddStartItems(int classIndex)
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
                ItemMetaData metaData = new(10);
                playerInven.Add(Creature.arrow, metaData);
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

