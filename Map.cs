public class Map
{
    public static Map? Current;
    private Program instance;
    public static Random rnd = new Random();
    private object[] content;
    public const string roadString = "·";
    public const string emptyString = "-";
    public const string nextMapString = "+";
    private bool isMovingUpward = true;
    private int playerPos;
    private Map? nextMap;
    public Map(int length, Program instance)
    {
        Current = this;
        this.instance = instance;
        playerPos = 0;
        content = new object[length];
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = emptyString;
        }
        content[length - 1] = nextMapString;
        instance.monster = new Monster("Bat", ClassName.Warrior, 1, 3, 1, 2, 5, 2); //Test!
        content[rnd.Next(2, length - 2)] = instance.monster;
    }
    private string ParseBlank(object? x)
        => emptyString;

    private string ParseVisible(object x)
    {
        if (x is Map)
        {
            return nextMapString;
        }
        if (x.ToString() == emptyString)
        {
            return roadString;
        }

        return x.ToString() ?? roadString;
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
        instance.ElaspeTurn();
    }
    private void CheckStepping()
    {
        object stepping = content[playerPos];
        switch (stepping)
        {
            case (object)nextMapString:
                NextMap();
                break;
            default:
                break;
        }

    }
    private void CheckFoward()
    {
        if (isAtLeftEnd || isAtRightEnd) return;
        object? foward = content[FowardIndex];
        if (foward is Monster && ((Monster)foward).Hp.IsAlive)
        {
            Entity.SetCurrentTarget((Entity)foward, Player.instance);
        }
    }

    private void NextMap()
    {
        nextMap = new Map(rnd.Next(4, 10), instance);
        instance.currentMap = nextMap;
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
            return fowardContent.ToString() != nextMapString && fowardContent.ToString() != emptyString;
        }

    }
}