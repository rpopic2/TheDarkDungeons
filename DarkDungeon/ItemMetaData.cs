public class ItemMetaData
{
    public int stack = 1;
    public int magicCharge = 0;
    public int poison = 0;

    private const int REQEXP_BASE = 5;
    public int Mastery { get; private set; } = 0;
    private int reqExp = REQEXP_BASE;
    private int curExp = 0;

    public ItemMetaData(int stack = 1)
    {
        this.stack = stack;
    }

    public int CurExp { get => curExp; }
    public void GainExp()
    {
        curExp++;
        IO.pr($"경험치 {curExp}/{reqExp}");
        if (curExp >= reqExp)
        {
            curExp -= reqExp;
            Mastery++;
            reqExp += REQEXP_BASE * Mastery;
            IO.rk("이 무기에 좀더 손이 익은 것 같다");
        }
    }
    public override string ToString()
    {
        return $"갯수:{stack}, 숙련도:{Mastery}, 숙련도 경험치:{curExp}/{reqExp}, 마법 충전량:{magicCharge}, 독:{poison}";
    }

}
