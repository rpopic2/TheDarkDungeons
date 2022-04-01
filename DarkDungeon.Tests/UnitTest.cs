using Xunit;
using Entities;
using System.Linq;

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

    [Fact]
    public void TokenCreationTest()
    {
        Tokens tokens = new(3);
        tokens.Add(TokenType.Offence);
        Assert.Equal("토큰 : (", tokens.ToString());
        tokens.Add(TokenType.Defence);
        Assert.Equal("토큰 : ( [", tokens.ToString());
        tokens.Add(TokenType.Charge);
        Assert.Equal("토큰 : ( [ <", tokens.ToString());
        tokens.Remove(TokenType.Defence);
        tokens.Add(TokenType.Charge);
        Assert.Equal("토큰 : ( < <", tokens.ToString());

    }
    [Fact]
    public void TokenPickup()
    {
        Player player = new("TestPlayer", default);
        player.tokens.Add(TokenType.Offence);
        Assert.Equal((byte)TokenType.Offence, player.tokens[0]);
        Assert.NotEqual((byte)TokenType.Defence, player.tokens[0]);
    }
}