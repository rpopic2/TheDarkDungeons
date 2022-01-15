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
    private string Parse(object? x)
    {
        if (x is null)
        {
            return blankString;
        }
        return x.ToString() ?? blankString;
    }
    private string Parse2(object? x)
    {
        if (x is null)
        {
            return roadString;
        }
        return x.ToString() ?? roadString;
    }
    public override string ToString()
    {
        string[] array = Array.ConvertAll(content, new Converter<object?, string>(Parse));
        array[playerPos + 1] = Parse2(content[playerPos + 1]);
        return string.Join(" ", array);
    }

    public void MoveUp()
    {
        content[playerPos] = null;
        playerPos++;
        content[playerPos] = Player.instance;
    }
}