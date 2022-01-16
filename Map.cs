public class Map
{
    public static Map Current = default!;
    public static Random rnd = new Random();
    private string[] tiles;
    public ref readonly string[] Tiles
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
        //Spawn(Player.instance);
        Program.instance.monster = monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3);
        //Spawn(monster);
        //mobPos = rnd.Next(2, length - 2);
    }
    public void UpdateMoveable(Moveable mov)
    {
        Position pos = mov.position;
        moveables[pos.oldX] = null;
        moveables[pos.x] = mov;
    }
    public override string ToString()
    {
        Moveable mov = Player.instance;
        int front = mov.position.Front;
        string[] result = NewEmptyArray(length, MapSymb.invisible);
        bool success2 = tiles.TryGet(front, out string? obj2);
        if (success2) result[front] = obj2!;

        bool success = moveables.TryGet(front, out Moveable? obj);
        if (success) result[front] = obj!.ToString()!;

        result[mov.position.x] = Player.instance.ToString();
        //result[monster.Position] = monster.ToString();
        return string.Join(" ", result);
    }

    private void MoveMob()
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

    public static string[] NewEmptyArray(int length, string fill)
    {
        string[] result = new string[length];
        for (int i = 0; i < length; i++)
        {
            result[i] = fill;
        }
        return result;
    }
}

public static class MapSymb
{
    public const string road = "·";
    public const string invisible = "-";
    public const string portal = "+";
}