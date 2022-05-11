public partial class Player
{
    public override void LetSelectBehaviour()
    {
        do
        {
            Selection();
        } while (CurAction.CurrentBehav is null);

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
            case ConsoleKey.L:
                if (info.Modifiers == ConsoleModifiers.Control) IO.Redraw();
                else if (CanMove(Position.MOVERIGHT)) SelectBasicBehaviour(0, Position.MOVERIGHT.x, (int)Position.MOVERIGHT.facing);
                break;
            case ConsoleKey.NumPad0:
            case ConsoleKey.D0:
                IO.Redraw();
                break;
            case ConsoleKey.LeftArrow:
            case ConsoleKey.H:
                if (CanMove(Position.MOVELEFT)) SelectBasicBehaviour(0, Position.MOVELEFT.x, (int)Position.MOVELEFT.facing);
                break;
            case ConsoleKey.N:
            case ConsoleKey.OemPeriod:
            case ConsoleKey.Delete: //Rest
                SelectBasicBehaviour(1, 0, -1); //x, y로 아무거나 넣어도 똑같음
                break;
            case ConsoleKey.Z: //상호작용
            case ConsoleKey.Enter: //상호작용
                if (UnderFoot is not null) SelectBasicBehaviour(2, 0, 0);
                break;
            default:
                DefaultSwitch(info);
                break;
        }
    }
    private void DefaultSwitch(ConsoleKeyInfo key)
    {
        bool found = IO.chk(key.KeyChar, Inventory.INVENSIZE, out int i);
        if (!found) IO.chkp(key.Key, Inventory.INVENSIZE, out i);
        if (found)
        {
            if (i >= Inven.Count)
            {
                SelectBehaviour(bareHand);
                return;
            }
            else if (Inven[i] is Item item)
            {
                SelectBehaviour(item);
                return;
            }
        }

        switch (key.KeyChar)
        {
            case '.':
                SelectBasicBehaviour(1, 0, -1);
                break;
            case 'i':
            case '*':
                IO.DrawInventory();
                break;
            case '/':
            case 'm':
                IO.ShowStats();
                break;
            case '?':
            case '5':
                IO.ShowHelp();
                break;
        }
    }
}
