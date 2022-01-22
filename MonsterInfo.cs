public readonly struct MonsterInfo
{
    public readonly string Name;
    public readonly ClassName ClassName;
    public readonly (int, float, float) Hp;
    public readonly PointInfo Cap;
    public readonly PointInfo Sol;
    public readonly PointInfo Lun;
    public readonly PointInfo Con;
    public readonly PointInfo KillExp;

    public MonsterInfo(string name, ClassName className, (int, float, float) hp, PointInfo cap, PointInfo sol, PointInfo lun, PointInfo con, PointInfo killExp)
    {
        Name = name;
        ClassName = className;
        Hp = hp;
        Cap = cap;
        Sol = sol;
        Lun = lun;
        Con = con;
        KillExp = killExp;
    }
}