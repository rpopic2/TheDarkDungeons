public class Map
{
    public static Map Current = default!;
    public static Random rnd = new Random();
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Moveable?[] moveables;
    public ref readonly Moveable?[] Moveables
    => ref moveables;
    public readonly int length;
    private Monster monster;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyMap(length, MapSymb.road);
        moveables = new Moveable[length];

        tiles[length - 1] = MapSymb.portal;
        moveables[0] = Player.instance;
        Program.instance.monster = monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3);
        //mobPos = rnd.Next(2, length - 2);
    }
    public void UpdateMoveable(Moveable mov)
    {
        Position pos = mov.Pos;
        if (!mov.IsAlive)
        {
            moveables[pos.x] = null;
            return;
        }
        if (moveables[pos.oldX] == mov) moveables[pos.oldX] = null;
        moveables[pos.x] = mov;
    }
    public override string ToString()
    {
        Moveable mov = Player.instance;
        int front = mov.Pos.Front;
        char[] result = NewEmptyMap(length, MapSymb.invisible);
        bool success2 = tiles.TryGet(front, out char obj2);
        if (success2) result[front] = obj2!;

        bool success = moveables.TryGet(front, out Moveable? obj);
        if (success) result[front] = obj!.ToChar();

        result[mov.Pos.x] = Player.instance.ToChar();
        return string.Join(" ", result);
    }
    public static void NewMap()
    {
        Current = new Map(rnd.Next(4, 10));
    }

    private static char[] NewEmptyMap(int length, char fill)
    {
        char[] result = new char[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = fill;
        }
        return result;
    }
}

public static class MapSymb
{
    public const char road = '·';
    public const char invisible = '-';
    public const char portal = '+';
}