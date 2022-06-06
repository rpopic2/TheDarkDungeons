using System;
using Xunit;
public class DamageTypeTest : IDisposable
{
    Map? map;
    Player? _player;
    Player player => _player!;
    TestMonster? _testMon;
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
        player.Inven.Add(Creature.sword);
        testMon.GiveItemOld(Creature.sword);
        player.SetAction(Creature.sword, 0, BASE_DMG);//Slash dmg
        testMon.SetAction(Creature.sword, 0);//Non-vul

        Program.ElaspeTurn();

        int expectedHp = MON_MAXHP - BASE_DMG;
        Assert.Equal(expectedHp, testMon.GetHp().Cur);

    }
    [Fact]
    public void SlashToVul()
    {
        player.Inven.Add(Creature.sword);
        testMon.GiveItemOld(Creature.sword);
        player.SetAction(Creature.sword, 0, BASE_DMG);//slash dmg
        testMon.SetAction(Creature.basicActions, 1);//vul

        Program.ElaspeTurn();
        int expectedHp = MON_MAXHP - BASE_DMG.ToVul();
        Assert.Equal(expectedHp, testMon.GetHp().Cur);
    }
    private void SetupDamageTest(ItemOld item1, int skill1, ItemOld item2, int skill2)
    {
        player.Inven.Add(item1);
        player.SetAction(item1, skill1, BASE_DMG);//slash dmg
        testMon.GiveItemOld(item2);
        testMon.SetAction(item2, skill2, BASE_DEF);//slash def 5
        Program.ElaspeTurn();
    }
    private void AssertEffective()
    {
        _AssertDamage(BASE_DMG.ToUnVul());
    }
    private void AssertNormal()
    {
        _AssertDamage(BASE_DMG);
    }
    private void AssertNotEffective()
    {
        _AssertDamage(BASE_DMG.ToVul());
    }
    private void _AssertDamage(int originalDmg)
    {
        int damageDelt = originalDmg - BASE_DEF;
        int expectedHp = MON_MAXHP - damageDelt;
        Assert.Equal(expectedHp, testMon.GetHp().Cur);
    }
    [Fact]
    public void SlashToSlash() //effective
    {
        SetupDamageTest(Creature.sword, 0, Creature.sword, 1);
        AssertEffective();
    }
    [Fact]
    public void SlashToThrust() //normal
    {
        SetupDamageTest(Creature.sword, 0, Creature.bareHand, 1);
        AssertNormal();
    }
    [Fact]
    public void SlashToMagic() //uneffective
    {
        SetupDamageTest(Creature.sword, 0, Creature.wand, 1);
        AssertNotEffective();
    }
    [Fact]
    public void ThrustToSlash() //uneffective
    {
        SetupDamageTest(Creature.dagger, 1, Creature.sword, 1);
        AssertNotEffective();
    }
    [Fact]
    public void ThrustToThrust()//effective
    {
        SetupDamageTest(Creature.dagger, 1, Creature.bareHand, 1);
        AssertEffective();
    }
    [Fact]
    public void ThrustToMagic()
    {
        SetupDamageTest(Creature.dagger, 1, Creature.wand, 1);
        AssertNormal();
    }
    [Fact]
    public void MagicToSlash()
    {
        SetupDamageTest(Creature.staff, 1, Creature.basicActions, 1);
        SetupDamageTest(Creature.staff, 0, Creature.sword, 1);
        int firstDamageDelt = BASE_DMG - BASE_DEF;
        int secondDamageDelt = BASE_DMG - BASE_DEF;
        Assert.Equal(testMon.GetHp().Max - firstDamageDelt - secondDamageDelt, testMon.GetHp().Cur);
    }
    [Fact]
    public void MagicToThrust()
    {
        SetupDamageTest(Creature.staff, 1, Creature.basicActions, 1);
        SetupDamageTest(Creature.staff, 0, Creature.bareHand, 1);
        int firstDamageDelt = BASE_DMG.ToVul() - BASE_DEF;//magic -> thrust not effective
        int secondDamageDelt = BASE_DMG - BASE_DEF;//normal -> thrust normal
        Assert.Equal(testMon.GetHp().Max - firstDamageDelt - secondDamageDelt, testMon.GetHp().Cur);
    }
    [Fact]
    public void MagicToMagic()
    {
        SetupDamageTest(Creature.staff, 1, Creature.basicActions, 1);
        SetupDamageTest(Creature.staff, 0, Creature.wand, 1);
        int firstDamageDelt = BASE_DMG.ToUnVul() - BASE_DEF;//magic -> magic effective
        int secondDamageDelt = BASE_DMG - BASE_DEF;//normal -> magic normal
        Assert.Equal(testMon.GetHp().Max - firstDamageDelt - secondDamageDelt, testMon.GetHp().Cur);
    }
}
