public class Energy : GamePoint
{
    public Energy(int max) : base(max, GamePointOption.Reserving)
    {
    }
    public void Gain()
    {
        Cur++;
    }
    public void Consume(out bool success)
    {
        if (Cur <= 0)
        {
            IO.rk("기력이 없습니다.");
            success = false;
            return;
        }
        success = true;
        Cur--;
    }
}
