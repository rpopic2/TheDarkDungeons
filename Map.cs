public class Map
{

    public static Map? Current;
    private Program program;
    public static Random rnd = new Random();
    private object[] content;
    private bool isMovingUpward = true;
    private int playerPos;
    private Map? nextMap;
    public Map(int length, Program instance)
    {
        Current = this;
        this.program = instance;
        playerPos = 0;
        content = new object[length];
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = MapSymb.empty;
        }
        content[length - 1] = MapSymb.next;
        instance.monster = new Monster("Bat", ClassName.Warrior, 1, 1, 1, 2, 1, 2, 3); //Test!
        content[rnd.Next(2, length - 2)] = instance.monster;
    }
    private string ParseBlank(object? x)
        => MapSymb.empty;

    private string ParseVisible(object x)
    {
        if (x is Map)
        {
            return MapSymb.next;
        }
        if (x.ToString() == MapSymb.empty)
        {
            return MapSymb.road;
        }

        return x.ToString() ?? MapSymb.road;
    }
    public override string ToString()
    {
        string[] result = Array.ConvertAll(content, new Converter<object, string>(ParseBlank));
        if (isMovingUpward && isAtRightEnd) goto result;
        if (!isMovingUpward && isAtLeftEnd) goto result;
        result[FowardIndex] = ParseVisible(content[FowardIndex]);
    result:
        result[playerPos] = "@";
        return string.Join(" ", result);
    }
    public void Move(int x)
    {
        isMovingUpward = x < 0 ? false : true;
        if (isAtRightEnd) return;
        if (CantMoveFoward) return;
        playerPos += x;
        CheckStepping();
        CheckFoward();
        IO.del(2);
        program.ElaspeTurn();
    }
    private void CheckStepping()
    {
        if (content[playerPos] == (object)MapSymb.next)
        {
            InitMap(nextMap, program);
        }
    }
    private void CheckFoward()
    {
        if (isAtLeftEnd || isAtRightEnd) return;
        object fowardObject = content[FowardIndex];
        if (fowardObject is Monster && ((Monster)fowardObject).IsAlive)
        {
            Entity.SetCurrentTarget((Entity)fowardObject, Player.instance);
        }
    }

    public static void InitMap(Map? map, Program prog)
    {
        map = new Map(rnd.Next(4, 10), prog);
        prog.currentMap = map;
    }
    private bool isAtLeftEnd
        => playerPos <= 0;
    private bool isAtRightEnd
        => playerPos >= content.Length - 1;
    private int FowardIndex
        => isMovingUpward ? playerPos + 1 : playerPos - 1;
    private bool CantMoveFoward
    {
        get
        {
            object fowardContent = content[FowardIndex];
            return fowardContent.ToString() != MapSymb.next && fowardContent.ToString() != MapSymb.empty;
        }

    }
}

public static class MapSymb
{
    public const string road = "·";
    public const string empty = "-";
    public const string next = "+";
    public const string player = "+";
}