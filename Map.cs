public class Map
{
    public static Map? Current;
    private Program instance;
    private Random rnd;
    private object[] content;
    public const string roadString = "·";
    public const string deadString = "_";
    public const string emptyString = "-";
    public const string nextMapString = "+";
    private bool isMovingFoward = true;
    private int playerPos;
    private Map? nextMap;
    public Map(int length, Program instance)
    {
        Current = this;
        this.instance = instance;
        rnd = new Random();
        playerPos = 0;
        content = new object[length];
        for (int i = 0; i < content.Length; i++)
        {
            content[i] = emptyString;
        }
        content[length - 1] = nextMapString;
        content[2] = instance.monster;
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
        if (isMovingFoward && isAtRightEnd) goto result;
        if (!isMovingFoward && isAtLeftEnd) goto result;
        result[FowardIndex] = ParseVisible(content[FowardIndex]);
    result:
        result[playerPos] = "@";
        return string.Join(" ", result);
    }

    public void MoveUp()
    {
        if (isAtRightEnd) return;
        isMovingFoward = true;
        if (content[FowardIndex]?.ToString() != emptyString) return;
        playerPos++;
        CheckStepping();
        CheckFoward();
    }

    public void MoveDown()
    {
        if (isAtLeftEnd) return;
        isMovingFoward = false;
        if (content[FowardIndex]?.ToString() != emptyString) return;
        playerPos--;
        CheckStepping();
        CheckFoward();

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
            IO.pr("HI");
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
        => isMovingFoward ? playerPos + 1 : playerPos - 1;
}