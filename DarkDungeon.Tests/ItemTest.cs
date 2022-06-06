using System;
using Xunit;
using Item;
public class ItemTest : TestTemplate
{
    [Fact]
    public void GiveItemTest()
    {
        Assert.False(player.HasItem<ShadowBow>());
        player.GiveItem(new ShadowBow());
        bool hasShadowBow = player.HasItem<ShadowBow>();
        Assert.True(hasShadowBow);
    }
    [Fact]
    public void GetItemAtTest()
    {
        ShadowBow bow = new();
        player.GiveItem(bow);
        Assert.Same(bow, player.GetItemAt(0));
        player.GiveItem(new Bolt());
        player.GiveItem(new Bolt());
        Assert.Equal(new Bolt(), player.GetItemAt(1));
        Assert.Equal(new BareHand(), player.GetItemAt(2));

    }
    [Fact]
    public void HasMultipleItems()
    {
        Assert.False(player.HasItem<ShadowBow>());
        player.GiveItem(new ShadowBow());
        Assert.False(player.HasItem<Bolt>());
        player.GiveItem(new Bolt());
        Assert.True(player.HasItem<ShadowBow>());
        Assert.True(player.HasItem<Bolt>());
    }
    [Fact]
    public void ItemStacks()
    {
        player.GiveItem(new Bolt());
        player.GiveItem(new Bolt());
        int stackOfBolt = player.GetStack<Bolt>();
        Assert.Equal(2, stackOfBolt);
    }
    [Fact]
    public void StackCannotBeLessThanOne()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Bolt(0));
    }
    [Fact]
    public void StackStruct()
    {
        Stack stack = 2;
        Assert.Equal(2, (int)stack);
        Assert.Throws<ArgumentOutOfRangeException>(() => stack = 0);
    }
    [Fact]
    public void StackOfIStackable()
    {
        Bolt bolt = new(2);
        Assert.Equal(2, (int)bolt.Stack);
        player.GiveItem(bolt);
        Assert.Equal(2, player.GetStack<Bolt>());
    }
    [Fact]
    public void StackOfNotIStackable()
    {
        player.GiveItem(new ShadowBow());
        Assert.Equal(1, player.GetStack<ShadowBow>());
    }
    [Fact]
    public void StackOfNonExistingItem()
    {
        Assert.Equal(0, player.GetStack<ShadowBow>());
        Assert.Equal(0, player.GetStack<Bolt>());
    }
    [Fact]
    public void RemoveItem()
    {
        player.GiveItem(new ShadowBow());
        player.RemoveItem<ShadowBow>();
        Assert.False(player.HasItem<ShadowBow>());
    }
    [Fact]
    public void RemoveItemStack()
    {
        player.GiveItem(new Bolt(3));
        player.RemoveItemStack<Bolt>(1);
        Assert.Equal(2, player.GetStack<Bolt>());
        player.RemoveItem<Bolt>();
        Assert.Equal(0, player.GetStack<Bolt>());
    }
    [Fact]
    public void RemoveItemDoesNotThrowError()
    {
        player.RemoveItem<Bolt>();//trying to remove when bolt does not exist in inventory.
    }
    ///
    /// Item specific tests
    ///
    [Fact]
    public void ShootShadowBow()
    {
        TestMonster testMon = new(new(2));
        player.GiveItem(new ShadowBow());
        player.GiveItem(new Bolt());
        player.GiveItem(new Bolt());
        OnTurn += player.GetItem<ShadowBow>().Throw;
        Assert.Equal(testMon.MaxHp, testMon.CurrentHp);

        Program.ElaspeTurn();
        Assert.Equal(1, player.GetStack<Bolt>());//consumes bolt
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTest()
    {
        TestMonster testMon = new(new(1));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Pierce;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTestOutOfRange()
    {
        TestMonster testMon = new(new(2));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Pierce;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.Equal(testMon.MaxHp, testMon.CurrentHp);
    }
    [Fact]
    public void ShadowDaggerTestThrow()
    {
        TestMonster testMon = new(new(3));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Throw;
        Program.ElaspeTurn();
        Assert.False(player.Energy.IsMax);
        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
        int tempCur = testMon.CurrentHp;
        Program.ElaspeTurn();
        Assert.Equal(tempCur, testMon.CurrentHp);
        Assert.False(player.HasItem<ShadowDagger>());
        Assert.True(testMon.HasItem<ShadowDagger>());
    }
    [Fact]
    public void ShadowDaggerTestBackstep()//->PositionTest.Shadowstep
    {
        TestMonster testMon = new(new(3, Facing.Left));
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Shadowstep;
        Program.ElaspeTurn();
        Assert.Equal(new Position(4, Facing.Left), player.Pos);
        Assert.False(player.Energy.IsMax);
    }
    [Fact]
    public void ShadowstepNoEnergyConsumeOnNoTarget()//->PositionTest.Shadowstep
    {
        player.GiveItem(new ShadowDagger());
        OnTurn += player.GetItem<ShadowDagger>().Shadowstep;
        Program.ElaspeTurn();
        Assert.Equal(new(), player.Pos);
        Assert.True(player.Energy.IsMax);
    }
    [Fact]
    public void MonsterHasItem()
    {
        TestMonster testMon = new(new(2, Facing.Left));
        testMon.GiveItem(new ShadowDagger());
        OnTurn += testMon.GetItem<ShadowDagger>().Throw;
        Program.ElaspeTurn();
        Assert.NotEqual(player.MaxHp, player.CurrentHp);
    }
    ///
    /// Printing Inventory.
    ///
    [Fact]
    public void NewInvenToString()
    {
        Assert.Equal("(q|맨손)", player.InvenToString);
        player.GiveItem(new ShadowDagger());
        Assert.Equal("(q|그림자 단검)(w|맨손)", player.InvenToString);
        player.GiveItem(new Bolt());
        Assert.Equal("(q|그림자 단검)(w|석궁 볼트x1)(e|맨손)", player.InvenToString);
    }
    [Fact]
    public void InvenToStringStackedItems()
    {
        player.GiveItem(new Bolt());
        Assert.Equal("(q|석궁 볼트x1)(w|맨손)", player.InvenToString);
        player.GiveItem(new Bolt());
        Assert.Equal("(q|석궁 볼트x2)(w|맨손)", player.InvenToString);
    }
    [Fact]
    public void PrintSkillList()
    {
        ShadowDagger dagger = new();
        Assert.Equal("그림자 단검 | (q|찌르기)(w|던지기)", dagger.ToSkillString());
        //wip
    }
    [Fact]
    public void UseSkillByInput()
    {
        TestMonster testMon = new(new(1, Facing.Left));
        ShadowDagger dagger = new();
        player.GiveItem(dagger);

        IO2.Press('q');
        Assert.Equal(dagger, IO2.SelectedItem);
        Assert.Equal(testMon.MaxHp, testMon.CurrentHp);
        IO2.Press('q');

        Assert.NotEqual(testMon.MaxHp, testMon.CurrentHp);
        Assert.Null(IO2.SelectedItem);

    }
    [Fact]
    public void GetSkillByIndex()
    {
        ShadowDagger dagger = new();
        Assert.Equal(dagger.Pierce, dagger.GetSkillAt(0));
        Assert.Equal(dagger.Throw, dagger.GetSkillAt(1));
    }
    // [Fact]
    // public void SelectItemFromInven()
    // {
    //     Assert.Equal("(q|맨손)", Output.LastWrite);
    //     player.GiveItem(new ShadowDagger());
    //     Assert.Equal("(q|그림자 단검)(w|맨손)", Output.LastWrite);
    //     // Output.Press('q');
    //     // Assert.Equal("", Output.LastWrite);
    //     //wip
    // }
}