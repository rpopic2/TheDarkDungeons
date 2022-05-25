using System;
using Xunit;
public class DamageTypeTest : IDisposable
{
    Map? map;
    Player? _player;
    TestMonster? _testMon;
    Player player => _player!;
    TestMonster testMon => _testMon!;
    private const int MON_MAXHP = 20;
    private const int BASE_DMG = 10;
    private const int BASE_DEF = 5;
    public DamageTypeTest()
    {
        map = new(3, false);
        _player = Player._instance = new Player("test");
        _testMon = new(new(1));
        _testMon.SetHp(20);
        player.Inven.Add(Creature.sword);
        testMon.GiveItem(Creature.sword);
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
        Program.OnTurn = null;
        _testMon = null;
    }
    [Fact]
    public void SlashToNonVul()
    {
        player.CurAction.Set(Creature.sword, 0, BASE_DMG);//Slash dmg
        testMon.SetAction(Creature.sword, 0);//Non-vul

        Program.ElaspeTurn();

        int expectedHp = MON_MAXHP - BASE_DMG;
        Assert.Equal(expectedHp, testMon.GetHp().Cur);

    }
    [Fact]
    public void SlashToVul()
    {
        player.CurAction.Set(Creature.sword, 0, BASE_DMG);//slash dmg
        testMon.SetAction(Creature.basicActions, 1);//vul

        Program.ElaspeTurn();
        int expectedHp = MON_MAXHP - BASE_DMG.ToVul();
        Assert.Equal(expectedHp, testMon.GetHp().Cur);
    }
    [Fact]
    public void SlashToSlash()
    {
        player.CurAction.Set(Creature.sword, 0, BASE_DMG);//slash dmg
        testMon.CurAction.Set(Creature.sword, 1, BASE_DEF);//slash def 5

        Program.ElaspeTurn();
        int damageDelt = BASE_DMG.ToUnVul() - BASE_DEF;
        int expectedHp = MON_MAXHP - damageDelt;
        Assert.Equal(expectedHp, testMon.GetHp().Cur);
    }
    [Fact]
    public void SlashToThrust()
    {
        player.CurAction.Set(Creature.sword, 0, BASE_DMG);//slash dmg
        testMon.CurAction.Set(Creature.bareHand, 1, BASE_DEF);//thrust def 5

        Program.ElaspeTurn();
        int damageDelt = BASE_DMG - BASE_DEF;
        int expectedHp = MON_MAXHP - damageDelt;
        Assert.Equal(expectedHp, testMon.GetHp().Cur);
    }
}
