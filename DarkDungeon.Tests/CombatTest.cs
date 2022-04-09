using System.Collections.Generic;
using Entities;
using Xunit;

public class CombatTest
{
    [Fact]
    public void Movement()
    {
        Player._instance = new("Tester");
        Player player = Player.instance;
        Map map = new(5, false);
        Assert.Equal(new(0, 0), player.Pos);

        //move foward
        player.Move(1);
        Assert.Equal(new(1, 0), player.Pos);

        //move backward
        player.Move(-1);
        Assert.Equal(new(0, Facing.Back), player.Pos);

        //cannot move further than 0
        player.Move(-1);
        Assert.Equal(new(0, Facing.Back), player.Pos);

        //moving further than max moves you to next room
        player.Move(4);
        Assert.Equal(new(0, 0), player.Pos);
    }
    [Fact]
    public void TestOnNewTurn()
    {
        //Setup
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out Map map);

        //1. New Turn. update its targets
        _StartTurn(map);
        Assert.Equal(mob, player.Target);
        Assert.Equal(player, mob.Target);

        Assert.Equal(0, player.Stance.amount);
        Assert.Equal(Stance.None, player.Stance.stance);
        Assert.Equal(0, mob.Stance.amount);
        Assert.Equal(Stance.None, mob.Stance.stance);
    }
    [Fact]
    public void TestSkillSelect()
    {
        Player player = _SetupPlayer();
        Item bareHand = Item.bareHand;

        _SelectSkill(player, bareHand, 0);
        Assert.Equal(Stance.Offence, player.Stance.stance);
        Assert.InRange(player.Stance.amount, Stat.MIN, player.GetStat(bareHand.skills[0].statName));
    }
    [Fact]
    public void RestTest()
    {
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out _);

        player.Rest(TokenType.Offence);
        mob.Rest(TokenType.Offence);
        Assert.Equal(Stance.Charge, player.Stance.stance);
        Assert.Equal(Stance.Charge, mob.Stance.stance);
    }
    [Fact]
    public void ChargeTest()
    {
        Player player = _SetupPlayer();
        _SelectSkill(player, Item.staff, 1);
        Assert.Equal(Stance.Charge, player.Stance.stance);
    }

    //Combats
    [Fact]
    public void PlayerAtksMobAtks()
    {
        (Player player, Monster mob) = _DoCombat(0, 0);
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void PlayerAtksMobDefs()
    {
        (Player player, Monster mob) = _DoCombat(0, 1);
        if (mob.Stance.amount >= player.Stance.amount) Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        else Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void PlayerDefsMobAtks()
    {
        (Player player, Monster mob) = _DoCombat(1, 0);
        if (player.Stance.amount >= mob.Stance.amount) Assert.Equal(player.Hp.Max, player.Hp.Cur);
        else Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
    }
    [Fact]
    public void PlayerDefsMobDefs()
    {
        (Player player, Monster mob) = _DoCombat(1, 1);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void PlayerAtksMobRests()
    {
        (Player player, Monster mob) = _SetupCombat(out Map map);
        _SelectSkill(player, Item.sword, 0);
        mob.Rest(TokenType.Offence);
        _ElaspeTurn(map);
        Assert.Equal(Stance.Offence, player.Stance.stance);
        Assert.Equal(Stance.Charge, mob.Stance.stance);

        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
    }
    [Fact]
    public void PlayerDefsMobRests()
    {
        (Player player, Monster mob) = _SetupCombat(out Map map);
        _SelectSkill(player, Item.sword, 1);
        mob.Rest(TokenType.Offence);
        _ElaspeTurn(map);
        Assert.Equal(Stance.Defence, player.Stance.stance);
        Assert.Equal(Stance.Charge, mob.Stance.stance);
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
    }
    [Fact]
    public void PlayerRestsMobAtks()
    {
        (Player player, Monster mob) = _SetupCombat(out Map map);
        _SelectSkill(mob, Item.sword, 0);
        player.Rest(TokenType.Offence);
        _ElaspeTurn(map);
        Assert.Equal(Stance.Offence, mob.Stance.stance);
        Assert.Equal(Stance.Charge, player.Stance.stance);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
        Assert.NotEqual(player.Hp.Max, player.Hp.Cur);
    }
    [Fact]
    public void PlayerRestsMobDefs()
    {
        (Player player, Monster mob) = _SetupCombat(out Map map);
        _SelectSkill(mob, Item.sword, 1);
        player.Rest(TokenType.Offence);
        _ElaspeTurn(map);
        Assert.Equal(Stance.Defence, mob.Stance.stance);
        Assert.Equal(Stance.Charge, player.Stance.stance);
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        Assert.Equal(mob.Hp.Max, mob.Hp.Cur);
    }
    [Fact]
    public void PlayerAtksMobCharges()
    {
        (Player player, Monster mob) = _SetupCombat(out Map map);
        _SelectSkill(player, Item.staff, 0);
        _SelectSkill(mob, Item.staff, 1);
        _ElaspeTurn(map);
        Assert.Equal(Stance.Offence, player.Stance.stance);
        Assert.Equal(Stance.Charge, mob.Stance.stance);
        Assert.Equal(player.Hp.Max, player.Hp.Cur);
        Assert.NotEqual(mob.Hp.Max, mob.Hp.Cur);
    }

    ///Private Methods

    ///<summary> Always setup player first</summary>
    private Player _SetupPlayer()
    {
        Player._instance = new("Tester");
        Player player = Player.instance;
        player.tokens.Add(TokenType.Offence);
        player.tokens.Add(TokenType.Defence);
        player.tokens.Add(TokenType.Charge);
        return player;
    }
    ///<summary> Always setup player first</summary>
    private Monster _SetupMonser(out Map map)
    {
        map = new(5, false);
        MonsterData lunData = Monster.lunatic;
        map._Spawn(lunData, new(1, Facing.Back));
        return ((Monster)map.MoveablePositions[1]!);
    }
    private void _StartTurn(Map map)
    {
        foreach (Fightable item in map.Fightables)
        {
            item.OnTurnEnd();
        }
    }
    private void _SelectSkill(Inventoriable caster, Item item, int skill)
    {
        try { caster.SelectSkill(item, skill); } catch (System.InvalidOperationException) { }
    }
    private void _ElaspeTurn(Map map)
    {
        foreach (Fightable item in map.Fightables)
        {
            try { item.TryAttack(); } catch (System.InvalidOperationException) { }
        }
    }
    private (Player, Monster) _SetupCombat(out Map map)
    {
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out map);
        _StartTurn(map);
        return (player, mob);
    }
    private (Player, Monster) _DoCombat(int playerSkill, int mobSkill)
    {
        Player player = _SetupPlayer();
        Monster mob = _SetupMonser(out Map map);
        _StartTurn(map);
        _SelectSkill(player, Item.bareHand, playerSkill);
        _SelectSkill(mob, Item.bareHand, mobSkill);
        _ElaspeTurn(map);
        return (player, mob);
    }
}