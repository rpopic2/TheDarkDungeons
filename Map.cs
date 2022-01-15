public class Map
{
    private Program instance;
    private Random rnd;
    private object?[] content;
    private const string roadString = "·";
    private const string blankString = "-";
    private const string nextMapString = "+";
    private bool isMovingFoward = true;
    private int playerPos;
    private Map? nextMap;
    public Map(int length, Program instance)
    {
        rnd = new Random();
        content = new object[length];
        playerPos = 0;
        content[length - 1] = nextMapString;
        this.instance = instance;
        //content[3] = instance.
    }
    private string ParseBlank(object? x)
        => blankString;

    private string ParseVisible(object? x)
    {
        if (x is Map)
        {
            return nextMapString;
        }
        if (x is null)
        {
            return roadString;
        }

        return x.ToString() ?? roadString;
    }
    public override string ToString()
    {
        string[] result = Array.ConvertAll(content, new Converter<object?, string>(ParseBlank));
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
        playerPos++;
        CheckStepping();
        CheckFoward();
        isMovingFoward = true;
    }

    public void MoveDown()
    {
        if (isAtLeftEnd) return;
        playerPos--;
        CheckStepping();
        CheckFoward();

        isMovingFoward = false;
    }
    public void CheckStepping()
    {
        object? stepping = content[playerPos];
        switch (stepping)
        {
            case (object)nextMapString:
                NextMap();
                break;
            default:
                break;
        }

    }
    public void CheckFoward()
    {
        if(isAtLeftEnd || isAtRightEnd) return;
        object? foward = content[FowardIndex];
        if (foward is Monster)
            Entity.SetCurrentTarget((Entity)foward, Player.instance);
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