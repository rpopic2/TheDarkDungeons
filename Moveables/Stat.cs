public class Stat
{
    public Stat(int sol, int lun, int con)
    {
        Sol = sol;
        Lun = lun;
        Con = con;
    }

    private int sol;
    private int lun;
    private int con;

    public int Sol { get => sol; internal set => sol = value; }
    public int Lun { get => lun; internal set => lun = value; }
    public int Con { get => con; internal set => con = value; }
    public ref int RefSol() => ref sol;
    public ref int RefLun() => ref lun;
    public ref int RefCon() => ref con;
}