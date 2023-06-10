public static class PlayerCreator
{
    public static Player New() {
        IO.ServerSocket?.pr("<char_select>");
        string name = ChooseNamePrompt();
        IO.pr($"{name}의 직업은?...");
        var classIndex = ChooseClassPrompt();
        var player = new Player(name);
        AddStartItems(player, classIndex);
        if (classIndex == Class.Free)
            player.SelectPickupStat(3);
        else
            player.Stat[(StatName)classIndex] += 3;
        return player;
    }

    static Player s_player => Player.Me;
    static readonly string[] classes = new string[] { "^r (전사)^/", "^g (암살자)^/", "^b (마법사)^/" };

    static (Class, string) PromptClass() {
        IO.sel(classes, out int classIndex, 0);
        Class classType;
        if (classIndex == -1)
            classType = Class.Free;
        else
            classType = (Class)classIndex;
        string className = classes.ElementAtOrDefault(classIndex) ?? "자유전직";
        return (classType, className);
    }

    static Class ChooseClassPrompt() {
    ClassSelect:
        var (classType, className) = PromptClass();
    Confirm:
        string classDesc = GetClassDesc(classType);
        IO.pr($"{className} : {classDesc}");
        Status stat = Status.BasicStatus;
        if (classType != Class.Free)
            stat[(StatName)classType] += 3;
        IO.pr($"기본 스탯 : {stat}");
        IO.ServerSocket?.pr($"<class_info>{className}:{classDesc}:{stat}");
        IO.pr("스페이스바 : 확인, x : 취소");
        var keyInfo = IO.rk(false);
        if (keyInfo.Key.IsCancel() || keyInfo.KeyChar == 'x') {
            IO.del(2);
            goto ClassSelect;
        }
        else if (!keyInfo.Key.IsOK())
            goto Confirm;
        IO.del();
        return classType;
    }

    static string ChooseNamePrompt() {
        if (!IO.IsInteractive)
            return "TestPlayer";
        string name;
        do {
            IO.pr("캐릭터의 이름은?...");
            name = IO.rl() ?? "Michael";
            IO.del(2);
        } while (name == string.Empty);
        return name;
    }

    static string GetClassDesc(Class className) => className switch {
        Class.Warrior => "근접 거리에서 용맹히 싸우는 직업이다. 방패의 패리로 적을 기절시키며 제압할 수 있다. 또한 높은 체력을 지니고 있어 안정적이다.",
        Class.Assassin => "적이 저항하지 못하고 본인이 당했는지도 모르게끔 암살한다. 체력이 적어 자칫 한번에 죽을 수 있다.",
        Class.Mage => "창의력을 발휘하여 강력한 마법을 쓰는 직업이다. 수많은 스킬 조합이 가능하지만 운용 난이도는 높은 편이다.",
        Class.Free => "초보 비추천. 스탯과 아이템을 당신이 직접 고를 수 있다.",
        _ => throw new ArgumentException("Unknown class type", nameof(className))
    };

    public static void SelectStartItem(Player player) {
        Corpse corpse = new('@', "누군가", new(Player.Me, "시체"));
        corpse.droplist.Content.Add(Creature.sword);
        corpse.droplist.Content.Add(Creature.shield);
        corpse.droplist.Content.Add(Creature.dagger);
        corpse.droplist.Content.Add(Creature.bow);
        ItemMetaData arrowMeta = new();
        arrowMeta.stack = 3;
        corpse.droplist.Content.Add(Creature.arrow);
        corpse.droplist.Content.Add(Creature.staff);
        corpse.droplist.Content.Add(Creature.magicBook);
        while (player.Inven.Count < 2) {
            IO.sel(corpse.droplist.Content.ToArray(), out int index, 0, true, "아이템 선택 : ");
            if (corpse.droplist[index] is ItemOld item) {
                player.Inven.Add(item);
                corpse.droplist.Remove(item);
            }
        }
    }

    public static void AddStartItems(Player player, Class classIndex) {
        var Inven = player.Inven;
        switch (classIndex) {
        case Class.Warrior:
            Inven.Add(Creature.sword);
            Inven.Add(Creature.shield);
            Inven.Add(Creature.torch);
            break;
        case Class.Assassin:
            Inven.Add(Creature.dagger);
            Inven.Add(Creature.bow);
            ItemMetaData metaData = new(10);
            Inven.Add(Creature.arrow, metaData);
            Inven.Add(Creature.torch);
            Inven.Add(Creature.assBareHand);
            break;
        case Class.Mage:
            Inven.Add(Creature.staff);
            Inven.Add(Creature.magicBook);
            Inven.Add(Creature.torch);
            break;
        case Class.Free:
            SelectStartItem(player);
            break;
        }
    }
}

public enum Class {
    Warrior,
    Assassin,
    Mage,
    Free
}
