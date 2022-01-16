public class Map
{
    public static Map Current = default!;
    public static Random rnd = new Random();
    private char[] tiles;
    public ref readonly char[] Tiles
        => ref tiles;
    private Moveable?[] moveables;
    public readonly int length;
    private Monster monster;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        tiles = NewEmptyArray(length, MapSymb.road);
        moveables = new Moveable[length];

        tiles[length - 1] = MapSymb.portal;
        moveables[0] = Player.instance;
        Program.instance.monster = monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3);
        //mobPos = rnd.Next(2, length - 2);
    }
    public void UpdateMoveable(Moveable mov)
    {
        Position pos = mov.position;
        if (moveables[pos.oldX] == mov) moveables[pos.oldX] = null;
        moveables[pos.x] = mov;
    }
    public override string ToString()
    {
        Moveable mov = Player.instance;
        int front = mov.position.Front;
        char[] result = NewEmptyArray(length, MapSymb.invisible);
        bool success2 = tiles.TryGet(front, out char obj2);
        if (success2) result[front] = obj2!;

        bool success = moveables.TryGet(front, out Moveable? obj);
        if (success) result[front] = obj!.ToChar()!;

        result[monster.position.x] = monster.ToChar();
        result[mov.position.x] = Player.instance.ToChar();
        return string.Join(" ", result);
    }

    public void MoveMob()
    {
        if (rnd.Next(2) == 1) monster.Move(-1);
        else monster.Move(1);
    }

    private void MonsterCheck(object? checkObject)
    {
        if (checkObject is Monster && ((Monster)checkObject).IsAlive)
        {
            Entity.SetCurrentTarget((Entity)checkObject, Player.instance);
        }
        else if (Player.instance.target is not null)
        {
            Entity.LoseTarget(Player.instance, Player.instance.target);
        }
    }
    public static void NewMap()
    {
        Current = new Map(rnd.Next(4, 10));
    }

    public static char[] NewEmptyArray(int length, char fill)
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