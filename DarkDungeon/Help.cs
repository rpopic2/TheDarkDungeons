public static class Help
{
    public static void ShowHelp()
    {
        Console.Clear();
        IO.pr("\n필수 키 도움말\n");
        IO.pr("\n 좌우 화살표 = 이동");
        IO.pr("\n q, w, e, r, t : 아이템 선택 (누르면 맨 아래에 선택지가 나옴)");
        IO.pr("\n . = 휴식 (행동은 기력을 소모함. 휴식으로 다시 채움)");
        IO.pr("\n---------------------------------------------------------------");

        IO.pr("\n알면 좋은 키 도움말\n");
        IO.pr("\n\n / = 내 정보 보기, i : 인벤토리, ? : 이 도움말 보기");
        IO.pr("\n x = 취소, spacebar = Ok, z = 상호작용");
        IO.pr("\n ctrl + L = 새로고침 (맵이 이상하면 누르기)");

        IO.pr("=> 먹히지 않는 키가 있으면 모바일이나 숫자패드 단축키로 시도해 보십시오.", __.bottom | __.newline);
        IO.pr("여기에서 m : 모바일 단축키 보기, 5 : 숫자패드 단축키 보기, 다른 키를 눌러 돌아가기", __.bottom);
        ConsoleKeyInfo consoleKeyInfo = IO.rk();
        if (consoleKeyInfo.KeyChar == 'm') ShowMobileHelp();
        else if (consoleKeyInfo.KeyChar == '5') ShowNumpadHelp();
        else IO.Redraw();
    }

    private static void ShowNumpadHelp()
    {
        Console.Clear();
        IO.pr("숫자패드 키 도움말 (Num Lock 끄고 플레이)\n");
        IO.pr("\n 좌우 화살표(4, 6) = 좌우 이동");
        IO.pr("\n End(1) = 1번 아이템(혹은 선택지), PgDn(2) = 2번 아이템, Home(7) = 3번 아이템, PgUp(9) = 4번 아이템, + = 5번 아이템 / * : 인벤토리");
        IO.pr("\n DEL = 휴식");
        IO.pr("\n / = 내 정보 보기, 5(NumLock 켜고) : 이 도움말 보기");
        IO.pr("\n - = 취소, Enter = Ok, z = 상호작용");
        IO.pr("\n 0(NumLock 켜고) = 새로고침 (맵이 이상하면 누르기)");
        IO.rk();
        IO.Redraw();
    }

    public static void ShowMobileHelp()
    {
        Console.Clear();
        IO.pr("모바일 키 도움말\n");
        IO.pr("\n h, l = 좌우 이동");
        IO.pr("\n q = 1번 아이템(혹은 선택지), w = 2번 아이템, e = 3번 아이템, r = 4번 아이템, t = 5번 아이템 / i : 인벤토리");
        IO.pr("\n n = 휴식");
        IO.pr("\n m = 내 정보 보기, ? : 이 도움말 보기");
        IO.pr("\n x = 취소, spacebar = Ok 상호작용");
        IO.pr("\n 0 = 새로고침 (맵이 이상하면 누르기)");

        IO.rk();
        IO.Redraw();
    }
    public static readonly Dictionary<char, string> s_examineDict = new Dictionary<char, string>{
        {'/', "정보 보기"},
        {MapSymb.corpse, "시체"},
        {MapSymb.playerCorpse, "당신의 시체"},
        {MapSymb.road, "아무것도 없는 바닥"},
        {MapSymb.pit, "구멍(아래층으로 내려간다)"},
        {MapSymb.door, "갈림길(다음 맵으로 넘어간다)"},
        {MapSymb.player, "당신의 위치"},
        {Bat.data.backwardChar, "박쥐"},
        {Bat.data.fowardChar, "박쥐"},
        {'i', "인벤토리"},
        {'h', "좌로 이동"},
        {'l', "우로 이동"},
        {'z', "상호작용"},
        {' ', "확인"},
        {Lunatic.data.fowardChar, "광신도"},
        {Lunatic.data.backwardChar, "광신도"},
        {Snake.data.fowardChar, "뱀"},
        {Snake.data.backwardChar, "뱀"},
        {Shaman.data.fowardChar, "정령술사"},
        {Shaman.data.backwardChar, "정령술사"},
        {Rat.data.backwardChar, "쥐"},
        {Rat.data.fowardChar, "쥐"},
        {QuietKnight.data.backwardChar, "조용한 기사(보스)"},
        {QuietKnight.data.fowardChar, "조용한 기사(보스)"},
    };
    public static void ShowHelpPrompt()
    {
        ConsoleKeyInfo info = IO.rk("?를 한번 더 눌러 게임 도움말 보기 / 궁금한 글자를 키보드에서 눌러 검색");
        char key = info.KeyChar;
        if (key == '?') ShowHelp();
        else IO.rk($"{key} : {s_examineDict.GetValueOrDefault(key, "찾을 수 없습니다")}");
        IO.Redraw();
    }
}
