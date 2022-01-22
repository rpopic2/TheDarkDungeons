public static class Rules
{
    public const string version = "0.2";
    public static readonly bool MapDebug = false;
    public static readonly bool SkipIntro = false;
    public const float vulMulp = 1.3f;

    //player
    public const int capBasic = 3;
    public const float capByLevel = 0.4f;
    public const float hpByLevel = 1f;

    //map
    public const int MapLengthMin = 5;
    public const int MapLengthMax = 8;
    public const float MapWidthByLevel = 0.5f;

    //mobs
    public const float mhpByLevel = 0.8f;
    public const float mexpByLevel = 0.3f;
    public const float msolByTurn = 0.015f;
    public const float mlunByTurn = 0.005f;
    public const float mcapByLevel = 0.3f;
}

public static class MapSymb
{
    public const char road = '·';
    public const char invisible = ' ';
    public const char portal = '+';
}