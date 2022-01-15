public class Map
{
    public static Map Current = default!;
    public static Random rnd = new Random();
    private string[] content;
    public readonly int length;
    private bool isMovingUpward = true;//to entity
    private int playerPos;//to entity
    private int mobPos;//to entity
    private Monster monster;
    public Map(int length)
    {
        Current = this;
        this.length = length;
        content = NewEmptyArray(length, MapSymb.road);

        content[length - 1] = MapSymb.portal;

        Program.instance.monster = monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3);
        //mobPos = rnd.Next(2, length - 2);
    }
    public override string ToString()
    {
        string[] result = NewEmptyArray(length, MapSymb.invisible);
        bool success = TryGet(FrontIndex, out string? obj);
        if (success) result[FrontIndex] = obj!;
        result[playerPos] = Player.instance.ToString();
        result[mobPos] = monster.ToString();
        return string.Join(" ", result);
    }
    public void Move(int x)
    {
        isMovingUpward = x < 0 ? false : true;
        if (isAtFrontEnd) return;
        if (CantMoveFoward) return;
        playerPos += x;
        CheckStepping();
        CheckAround();
        //MoveMob();
        IO.del(2);
        Program.instance.ElaspeTurn();
    }
    private void CheckStepping()
    {
        if (content[playerPos] == (object)MapSymb.portal)
        {
            InitMap();
        }
    }
    private void MoveMob()
    {
        if (rnd.Next(2) == 1) mobPos--;
        else mobPos++;
    }

    private void CheckAround()
    {
        MonsterCheck(this[BackIndex]);
        MonsterCheck(this[FrontIndex]);
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
    private bool isAtBackEnd
        => playerPos <= 0;
    private bool isAtFrontEnd
        => playerPos >= content.Length - 1;
    private int FrontIndex
        => isMovingUpward ? playerPos + 1 : playerPos - 1;
    private int BackIndex
    => isMovingUpward ? playerPos - 1 : playerPos + 1;
    private object FrontObject
        => content[FrontIndex];
    private bool CantMoveFoward
    {
        get
        {
            object frontObject = FrontObject;
            return frontObject.ToString() != MapSymb.portal && frontObject.ToString() != MapSymb.road;
        }

    }
    public object? this[int index]
    {
        get
        {
            if (index < 0 || index >= content.Length) return null;
            else return content[index];
        }
    }
    public static void InitMap()
    {
        Current = new Map(rnd.Next(4, 10));
    }
    public bool TryGet(int index, out string? obj)
    {
        obj = null;
        if (index < 0 || index >= content.Length) return false;
        else
        {
            obj = content[index];
            return true;
        }
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