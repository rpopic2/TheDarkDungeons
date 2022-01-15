public class Map
{
    public static Map Current = default!;
    public static Random rnd = new Random();
    private string[] content;
    public ref readonly string[] Content
        => ref content;
    public readonly int length;
    private bool isMovingUpward = true;//to entity
    public Entity?[] entityContent;
    public int playerPos;//to entity
    private int mobPos;//to entity
    private Monster monster;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        content = NewEmptyArray(length, MapSymb.road);
        entityContent = new Entity[length];

        content[length - 1] = MapSymb.portal;
        entityContent[0] = Player.instance;
        Program.instance.monster = monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3);
        //mobPos = rnd.Next(2, length - 2);
    }
    public override string ToString()
    {
        Movement mov = Player.instance.movement;
        int front = mov.FrontPosition;
        string[] result = NewEmptyArray(length, MapSymb.invisible);
        bool success2 = content.TryGet(front, out string? obj2);
        if (success2) result[front] = obj2!;

        bool success = entityContent.TryGet(front, out Entity? obj);
        if (success) result[front] = obj!.ToString()!;

        result[mov.Position] = Player.instance.ToString();
        //result[mobPos] = monster.ToString();
        return string.Join(" ", result);
    }

    private void MoveMob()
    {
        if (rnd.Next(2) == 1) mobPos--;
        else mobPos++;
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