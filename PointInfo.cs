public readonly struct PointInfo
{
    public readonly int BasePoint;
    public readonly float PointPerLevel;
    public readonly float PointPerTurn;

    public PointInfo(int basePoint, float pointPerLevel = default, float pointPerTurn = default)
    {
        BasePoint = basePoint;
        PointPerLevel = pointPerLevel;
        PointPerTurn = pointPerTurn;
    }
}