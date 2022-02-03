public class Stat
{
    public Stat(int sol, int lun, int con)
    {
        this.lun = lun;
        this.sol = sol;
        this.con = con;
    }

    public int sol;
    public int lun;
    public int con;
    public ref int RefSol() => ref sol;
    public ref int RefLun() => ref lun;
    public ref int RefCon() => ref con;
}