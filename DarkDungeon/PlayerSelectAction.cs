public partial class Player
{
    public override void LetSelectBehaviour()
    {
        if (!IO.IsInteractive) return;
        do
        {
            Selection();
        } while (CurrentBehaviour is null);

    }
    private void Selection()
    {
        ConsoleKeyInfo info = IO.rk();
        ConsoleKey key = info.Key;
        if (key.IsCancel())
        {
            DiscardItem();
            IO.Redraw();
            return;
        }
        switch (key)
        {
            case ConsoleKey.RightArrow:
                ChooseMove(Facing.Right);
                break;
            case ConsoleKey.L:
                if (info.Modifiers == ConsoleModifiers.Control)
                {
                    IO.Redraw();
                    break;
                }
                else goto default;
            case ConsoleKey.NumPad0:
            case ConsoleKey.D0:
                IO.Redraw();
                break;
            case ConsoleKey.LeftArrow:
                ChooseMove(Facing.Left);
                break;
            case ConsoleKey.Delete: //Rest
                SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                break;
            case ConsoleKey.Enter: //상호작용
                if (UnderFoot is not null) SelectBasicBehaviour(2, 0, 0);
                break;
            default:
                DefaultSwitch(info);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKeyInfo key) {
        bool found = IO.chk(key.KeyChar, Inventory.INVENSIZE, out int i);
        if (!found) IO.chkp(key.Key, Inventory.INVENSIZE, out i);
        if (found) ChooseIndex(i);

        switch (key.KeyChar) {
        case 'z': case '>': case ',':
            if (UnderFoot is not null) SelectBasicBehaviour(2, 0, 0);
            break;
        //movements hl
        case 'l':
            ChooseMove(Facing.Right);
            break;
        case 'h':
            ChooseMove(Facing.Left);
            break;
        case '.'://rest
        case 'n':
            SelectBasicBehaviour(1, 0, -1);
            break;
        case 'i':
        case '*':
            IO.DrawInventory();
            break;
        case 'u':
            IO.ShowStats();
            break;
        case '?':
        case '5':
            Help.ShowHelpPrompt();
            break;
        }
    }
    private void ChooseMove(Facing facing) {
        Position newDir = facing == Facing.Left ? Position.MOVELEFT : Position.MOVERIGHT;
        if (CanMove(newDir))
            SelectBasicBehaviour(0, newDir.x, (int)newDir.facing);
    }

    private void ChooseIndex(int i) {
        if (i >= Inven.Count) {
            SelectBehaviour(bareHand);
            return;
        }
        else if (Inven[i] is ItemOld item) {
            SelectBehaviour(item);
            return;
        }
    }
}
