using System;
using Xunit;
public class CombatTest : IDisposable
{
    Map? map;
    Player? _player;
    Player player => _player!;
    public CombatTest()
    {
        map = new(3, false);
        _player = Player._instance = new Player("test");
        Map.Current.UpdateFightable(player);
    }
    public void Dispose()
    {
        map = null;
        _player = Player._instance = null;
        Program.OnTurn = null;
    }

    [Fact]
    public void GainExpByHit()
    {
        player.PickupItem(Creature.dagger);
        TestMonster testMon = new(new(1, Facing.Right));
        player.SetAction(player.Inven[0]!, 0, 3);
        Program.OnTurn?.Invoke();
        Assert.NotEqual(0, player.Inven.GetMeta(Creature.dagger)!.CurExp);
    }
    [Fact]
    public void PreserveThrowableWeaponExps()
    {
        player!.PickupItem(Creature.dagger);

        player.SetAction(player.Inven[0]!, 1, 10);

        Shaman shaman = new(new(2));
        Program.OnTurn?.Invoke();
        Assert.False(shaman.IsAlive);

        player.SetAction(Creature.basicActions, 0, 2, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.NotNull(player.UnderFoot);

        Corpse corpse = (Corpse)player.UnderFoot!;
        ItemMetaData metaData = corpse.droplist.GetMeta(Creature.dagger)!;
        // Assert.Equal(1, metaData.CurExp);

        corpse.GetItemAndMeta(2, out ItemOld? item, out ItemMetaData? metaData2);
        player.PickupItem(item!, metaData2!);

        Assert.Equal(1, metaData2!.CurExp);

        Assert.Equal(1, player.Inven.GetMeta(Creature.dagger)!.CurExp);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SetDamageByCurActionSet(int damage)
    {
        player!.PickupItem(Creature.sword);
        player.SetAction(Creature.sword, 0, damage);
        TestMonster testMon = new(new(1, Facing.Right));
        testMon.GiveItemOld(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Program.OnTurn?.Invoke();
        Assert.Equal(testMon.GetHp().Max - damage, testMon.GetHp().Cur);
    }
    [Fact]
    public void MagicChargeTest()
    {
        TestMonster testMon = new(new(1, Facing.Left));
        testMon.SetStat(StatName.Con, 10);

        testMon.GiveItemOld(Creature.spiritStaff);

        player.SetAction(Creature.basicActions, 1);
        testMon.SetAction(Creature.spiritStaff, 1, 10);
        Program.ElaspeTurn();

        Assert.NotEqual(0, testMon.Inven.GetMeta(Creature.spiritStaff)!.magicCharge);

        // Assert.False(player.GetHp().IsMax);

    }
    [Fact]
    public void MagicAttackTest()
    {
        TestMonster testMon = new(new(1, Facing.Left));
        testMon.SetStat(StatName.Con, 10);

        player.Inven.Add(Creature.wand);
        testMon.GiveItemOld(Creature.spiritStaff);

        player.SetAction(Creature.basicActions, 1);
        testMon.SetAction(Creature.spiritStaff, 1, 10);
        Program.ElaspeTurn();

        player.SetAction(Creature.basicActions, 1);
        testMon.SetAction(Creature.spiritStaff, 0, 10);
        Program.ElaspeTurn();

        Assert.False(player.IsAlive);
        Assert.Equal(0, player.GetHp().Cur);
    }
    [Fact]
    public void MagicDefenceTest()
    {
        TestMonster testMon = new(new(1, Facing.Left));
        testMon.SetStat(StatName.Con, 10);

        player.Inven.Add(Creature.wand);
        testMon.GiveItemOld(Creature.spiritStaff);

        player.SetAction(Creature.basicActions, 1);
        testMon.SetAction(Creature.spiritStaff, 1, 10);
        Program.ElaspeTurn();

        player.SetAction(Creature.wand, 1, 10);
        testMon.SetAction(Creature.spiritStaff, 0, 10);
        Program.ElaspeTurn();

        Assert.True(player.IsAlive);
        Assert.NotEqual(0, player.GetHp().Cur);
    }
}
