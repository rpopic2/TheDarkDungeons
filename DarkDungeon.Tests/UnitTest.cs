using Xunit;

public class UnitTest
{
    [Fact]
    public void CardToStringOffensive()
    {
        Card card = new(3, Stats.Sol, true);
        Assert.Equal("(3s)", card.ToString());

        card = new(1, Stats.Sol, true);
        Assert.Equal("(1s)", card.ToString());

        card = new(1, Stats.Lun, true);
        Assert.Equal("(1L)", card.ToString());

        card = new(1, Stats.Con, true);
        Assert.Equal("(1*)", card.ToString());
    }
    [Fact]
    public void CardToStringdefensive()
    {
        Card card = new(3, Stats.Sol, false);
        Assert.Equal("[3s]", card.ToString());

        card = new(1, Stats.Sol, false);
        Assert.Equal("[1s]", card.ToString());

        card = new(1, Stats.Lun, false);
        Assert.Equal("[1L]", card.ToString());

        card = new(1, Stats.Con, false);
        Assert.Equal("[1*]", card.ToString());

        Assert.NotEqual("[1s]", card.ToString());
    }
}