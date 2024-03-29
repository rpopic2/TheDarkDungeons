public static class Rules
{
    public const float vulMulp = 1.3f;
    //map
    public static readonly int MapLengthMax = 25;//Console.BufferWidth - 1;
    public static readonly int MapLengthMin = MapLengthMax / 2;
    public const float MapWidthByLevel = 3f;
    public const float LEVEL_DIFFICULTY = 1.15f;
    public const float LEVEL_TO_BASE_HP_RATE = 0.7f;

}

public static class MapSymb
{
    public const char player = '@';
    public const char road = '.';
    public const char Empty = ' ';
    public const char pit = ',';
    public const char corpse = 'x';
    public const char playerCorpse = 'X';
    public const char door = '+';
}
