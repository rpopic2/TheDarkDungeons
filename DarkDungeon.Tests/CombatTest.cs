using System;
using Xunit;
public class CombatTest : IDisposable
{
    Map? map;
    Player? player;
    public CombatTest()
    {
        map = new(3, false);
        player = Player._instance = new Player("test");
    }
    public void Dispose()
    {
        map = null;
        player = Player._instance = null;
        Program.OnTurn = null;
    }

    [Fact]
    public void GainExpByHit()
    {
        player!.PickupItem(Creature.dagger);
        TestMonster testMon = new(new(1, Facing.Right));
        player!.CurAction.Set(player.Inven[0]!, Creature.dagger.skills[0], 3);
        Program.OnTurn?.Invoke();
        Assert.NotEqual(0, player.Inven.GetMeta(Creature.dagger).CurExp);
    }
    [Fact]
    public void PreserveThrowableWeaponExps()
    {
        player!.PickupItem(Creature.dagger);

        player.CurAction.Set(player.Inven[0]!, Creature.dagger.skills[1]);

        Shaman shaman = new(new(2));
        Program.OnTurn?.Invoke();
        Assert.False(shaman.IsAlive);

        player.CurAction.Set(Creature.basicActions, Creature.basicActions.skills[0], 2, (int)Facing.Right);
        Program.OnTurn?.Invoke();
        Assert.NotNull(player.UnderFoot);

        Corpse corpse = (Corpse)player.UnderFoot!;
        ItemMetaData metaData = corpse.droplist.GetMeta(Creature.dagger);
        Assert.Equal(1, metaData.CurExp);

        corpse.GetItemAndMeta(2, out Item? item, out ItemMetaData? metaData2);
        player.PickupItem(item, metaData2);

        Assert.Equal(1, metaData2.CurExp);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void SetDamageByCurActionSet(int damage)
    {
        player!.PickupItem(Creature.sword);
        player.CurAction.Set(Creature.sword, Creature.sword.skills[0], damage);
        TestMonster testMon = new(new(1, Facing.Right));
        testMon.GiveItem(Creature.sword);
        testMon.SetAction(Creature.sword, 0);
        Program.OnTurn?.Invoke();
        Assert.Equal(testMon.GetHp().Max - damage, testMon.GetHp().Cur);
    }
}
