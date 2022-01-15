public class Map
{
    private object?[] content;
    private const string roadString = "#";
    private const string blankString = " ";
    private bool isMovingFoward = true;
    private int playerPos;
    public Map()
    {
        content = new object[6];
        playerPos = 0;
        content[playerPos] = Player.instance;
    }
    private string ParseBlank(object? x)
    {
        if (x is null)
        {
            return blankString;
        }
        return x.ToString() ?? blankString;
    }
    private string ParseVisible(object? x)
    {
        if (x is null)
        {
            return roadString;
        }
        return x.ToString() ?? roadString;
    }
    public override string ToString()
    {
        string[] array = Array.ConvertAll(content, new Converter<object?, string>(ParseBlank));
        int visible = isMovingFoward ? playerPos + 1 : playerPos - 1;
        if (playerPos < content.Length - 1 && playerPos > 0) array[visible] = ParseVisible(content[visible]);
        return string.Join(" ", array);
    }

    public void MoveUp()
    {
        if (playerPos >= content.Length - 1) return;
        content[playerPos] = null;
        content[++playerPos] = Player.instance;
        isMovingFoward = true;
    }

    public void MoveDown()
    {
        if (playerPos <= 0) return;
        content[playerPos] = null;
        content[--playerPos] = Player.instance;
        isMovingFoward = false;
    }
}