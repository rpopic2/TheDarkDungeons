public class Movement
{
    public int Position { get; private set; }
    private Facing facing;
    public void Move(int x)
    {
        Map? current = Map.Current;
        facing = x < 0 ? Facing.Back : Facing.Front;
        bool success = current.Content.TryGet(FrontPosition, out string? obj);
        if (!success) return;
        current.entityContent[Position] = null;
        Position += x;
        current.entityContent[Position] = Player.instance;
        if (obj == MapSymb.portal)
        {
            current = null; //is this working?
            Map.NewMap();
            Position = 0;
        }
        IO.del(2);
        Program.instance.ElaspeTurn();

    }
    public int FrontPosition
        => facing == Facing.Front ? Position + 1 : Position - 1;
}