public class StatData
{
    public StatData(int sol, int lun, int con)
    {
        Sol = sol;
        Lun = lun;
        Con = con;
    }

    public int Sol { get; internal set; }
    public int Lun { get; internal set; }
    public int Con { get; internal set; }
}