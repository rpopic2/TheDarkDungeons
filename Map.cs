public class Map
{
    private object?[] content;
    private const string roadString = "#";
    private const string blankString = " ";
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
        if (playerPos < content.Length - 1) array[playerPos + 1] = ParseVisible(content[playerPos + 1]);
        return string.Join(" ", array);
    }

    public void MoveUp()
    {
        if (playerPos >= content.Length - 1) return;
        content[playerPos] = null;
        content[++playerPos] = Player.instance;
    }

    public void MoveDown()
    {
        if (playerPos <= 0) return;
        content[playerPos] = null;
        content[--playerPos] = Player.instance;
    }
}